using System;
using System.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models;
using GraphLabs.Site.Utils;
using Newtonsoft.Json;

namespace GraphLabs.Site.Controllers
{
    public class LabsController : Controller
    {
        private readonly GraphLabsContext _ctx = new GraphLabsContext();

        #region Отображение списка лабораторных работ
        public ActionResult Index()
        {
            this.AllowAnonymous(_ctx);

            var labs = (from g in _ctx.LabWorks
                        select g).ToArray();

            return View(labs);
        }

        [HttpPost]
        public string GetLabInfo(int Id)
        {
            this.AllowAnonymous(_ctx);

            JSONResultLabInfo result = new JSONResultLabInfo();
            
            var lab = _ctx.LabWorks.Find(Id);
            if (lab == null)
            {
                result.Result = 1;
                return JsonConvert.SerializeObject(result);
            }

            result.Result = 0;
            result.LabId = lab.Id;
            result.LabName = lab.Name;

            result.Tasks = new List<KeyValuePair<int, string>>();
            foreach (var t in lab.LabEntry.Tasks)
            {
                result.Tasks.Add(new KeyValuePair<int, string>((int)t.Id, t.Name));
            }

            result.Variants = new JSONResultVariants[lab.LabVariants.Count];
            int i = 1;
            foreach (var v in lab.LabVariants)
            {
                result.Variants[i - 1] = new JSONResultVariants();
                result.Variants[i - 1].VarId = v.Id;
                result.Variants[i - 1].VarName = v.Number;
                result.Variants[i - 1].TasksVar = new List<KeyValuePair<int, string>>();
                foreach (var t in v.TaskVariants)
                {
                    result.Variants[i - 1].TasksVar.Add(new KeyValuePair<int, string>((int)t.Task.Id, t.Number));
                }
                ++i;
            }           

            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public string DeleteLab(int Id)
        {
            this.AllowAnonymous(_ctx);

            var lab = _ctx.LabWorks.Find(Id);
            if (lab == null)
            {
                return "1";
            }

            return "2";
        }
        #endregion

        #region Создание и редактирование оаболаторной работы
        public ActionResult Create(long id = 0)
        {
            this.AllowAnonymous(_ctx);

            var tasks = (from g in _ctx.Tasks
                        select g).ToArray();
            
            CreateLabModel model = new CreateLabModel();
            model.Id = id;
            model.Tasks = new List<KeyValuePair<long, string>>();
            foreach (var t in tasks)
            {                
                model.Tasks.Add(new KeyValuePair<long, string>(t.Id, t.Name));
            }

            return View(model);
        }

        [HttpPost]
        public string Create(string Name, string DateFrom, string DateTo, string JsonArr, int Id = 0)
        {
            int[] tasksId = JsonConvert.DeserializeObject<int[]>(JsonArr);
            JSONResultCreateLab res = null;

            List<LabWork> existlab;
            if (Id == 0)
            {
                existlab = (from l in _ctx.LabWorks
                            where l.Name == Name
                            select l).ToList();
            }
            else
            {
                existlab = (from l in _ctx.LabWorks
                            where l.Name == Name
                            where l.Id != Id
                            select l).ToList();
            }
            if (existlab.Count != 0)
            {
                res = new JSONResultCreateLab { Result = 1, LabName = Name };
                return JsonConvert.SerializeObject(res);
            };

            LabWork lab;
            if (Id == 0)
            {
                lab = new LabWork();
            }
            else
            {
                try
                {
                    lab = _ctx.LabWorks.Find(Id);
                }
                catch (Exception)
                {
                    res = new JSONResultCreateLab { Result = 3, LabName = Name };
                    return JsonConvert.SerializeObject(res);
                }
            }
            
            lab.Name = Name;
            lab.AcquaintanceFrom = ParseDate(DateFrom);
            lab.AcquaintanceTill = ParseDate(DateTo);

            LabEntry entry;
            if (Id == 0)
            {
                entry = new LabEntry();
                lab.LabEntry = entry;
                entry.LabWork = lab;
            }
            else
            {
                entry = _ctx.LabEntries.Find(lab.LabEntry.Id);
                entry.Tasks.Clear();
            }
            
            foreach (var t in tasksId)
            {
                entry.Tasks.Add(_ctx.Tasks.Find(t));
            }
            if (Id == 0)
            {
                _ctx.LabWorks.Add(lab);
                _ctx.LabEntries.Add(entry);
            }
            else
            {
                _ctx.Entry(lab).State = EntityState.Modified;
                _ctx.Entry(entry).State = EntityState.Modified;
            }
            _ctx.SaveChanges();            

            deleteTasks(entry);

            if (Id == 0)
            {
                res = new JSONResultCreateLab { Result = 0, LabId = lab.Id, LabName = lab.Name };
            }
            else
            {
                res = new JSONResultCreateLab { Result = 4, LabId = lab.Id, LabName = lab.Name };
            }
            return JsonConvert.SerializeObject(res);
        }

        private DateTime ParseDate(string date)
        {
            if (date != "")
            {
                return DateTime.Parse(date);
            }
            else
            {
                return new DateTime(2000, 01, 01);
            };
        }

        private void deleteTasks(LabEntry entry)
        {
            var variants = (from t in _ctx.LabVariants
                            select t).ToList();
            variants = variants.Where(x => x.LabWork == entry.LabWork).ToList();
            for (int i = 0; i < variants.Count; ++i)
            {
                List<TaskVariant> coll = variants[i].TaskVariants.ToList();
                foreach (var t in coll)
                {
                    if (!entry.Tasks.Contains(t.Task))
                    {
                        variants[i].TaskVariants.Remove(t);
                    }
                }
                _ctx.Entry(variants[i]).State = EntityState.Modified;
            }
            _ctx.SaveChanges();
        }

        //В id передается результат запроса
        [HttpPost]
        public string EditLab(long Id)
        {
            CreateLabModel res = new CreateLabModel();
            var lab = _ctx.LabWorks.Find(Id);
            if (lab == null)
            {
                res.Id = 1;
                return JsonConvert.SerializeObject(res);
            }

            res.Id = 0;
            res.Name = lab.Name;
            res.AcquaintanceFrom = lab.AcquaintanceFrom;
            res.AcquaintanceTo = lab.AcquaintanceTill;
            res.Tasks = new List<KeyValuePair<long, string>>();

            foreach (var t in lab.LabEntry.Tasks)
            {
                res.Tasks.Add(new KeyValuePair<long, string>(t.Id, ""));
            }

            return JsonConvert.SerializeObject(res);
        }
        #endregion

        public ActionResult CreateVariant(long labId = 0, long varId = 0)
        {
            this.AllowAnonymous(_ctx);

            var lab = _ctx.LabWorks.Find(labId);
            if (lab == null)
            {
                return HttpNotFound();
            }

            CreateLabVariantModel model = new CreateLabVariantModel();
            model.id = lab.Id;
            model.varId = varId;
            model.Name = lab.Name;
            model.Variant = new Dictionary<KeyValuePair<long, string>, List<KeyValuePair<long, string>>>();

            foreach (var t in lab.LabEntry.Tasks)
            {
                var list = new List<KeyValuePair<long, string>>();
                foreach (var v in t.TaskVariants)
                {
                    list.Add(new KeyValuePair<long, string>(v.Id, v.Number));
                }
                model.Variant.Add(new KeyValuePair<long, string>(t.Id, t.Name), list);
            }

            return View(model);
        }

        [HttpPost]
        public string CreateVariant(int Id, string Number, string JsonArr, int variantId = 0)
        {
            this.AllowAnonymous(_ctx);

            int[] varId = JsonConvert.DeserializeObject<int[]>(JsonArr);

            int result = 0;

            var lab = _ctx.LabWorks.Find(Id);
            if (lab == null)
            {
                result = 1;
                return JsonConvert.SerializeObject(result);
            }

            var nameCollision = (from v in _ctx.LabVariants
                                 where v.Number == Number
                                 select v).ToList();
            nameCollision = nameCollision.Where(x => x.LabWork == lab).ToList();
            if (variantId != 0)
            {
                nameCollision = nameCollision.Where(x => x.Id != variantId).ToList();
            }
            if (nameCollision.Count != 0)
            {
                result = 2;
                return JsonConvert.SerializeObject(result);
            }

            LabVariant labVar = null;
            if (variantId == 0)
            {
                labVar = new LabVariant();
                labVar.LabWork = lab;
            }
            else
            {
                labVar = _ctx.LabVariants.Find(variantId);
                if (labVar == null)
                {
                    result = 3;
                    return JsonConvert.SerializeObject(result);
                }
            }

            labVar.Number = Number;
            labVar.TaskVariants.Clear();
            for (int i = 0; i < varId.Length; ++i)
            {
                labVar.TaskVariants.Add(_ctx.TaskVariants.Find(varId[i]));
            }

            if (variantId == 0)
            {
                _ctx.LabVariants.Add(labVar);
            }
            else
            {
                _ctx.Entry(labVar).State = EntityState.Modified;
                result = 4;
            }
            _ctx.SaveChanges();

            return JsonConvert.SerializeObject(result);
        }

        //В id передается результат, в Name - номер варианта
        [HttpPost]
        public string EditVariant(long varId)
        {
            JSONResultEditVariant res = new JSONResultEditVariant();
            var variant = _ctx.LabVariants.Find(varId);
            if (variant == null)
            {
                res.Result = 1;
                return JsonConvert.SerializeObject(res);
            }

            res.Result = 0;
            res.Name = variant.Number;
            res.Variant = new List<KeyValuePair<long, long>>();

            foreach (var t in variant.TaskVariants)
            {               
                res.Variant.Add(new KeyValuePair<long, long>(t.Task.Id, t.Id));
            }

            return JsonConvert.SerializeObject(res);
        }
    }
}
