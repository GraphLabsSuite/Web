using System;
using System.Data;
using System.Collections.Generic;
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
                result.Variants[i-1] = new JSONResultVariants();
                result.Variants[i-1].VarId = v.Id;
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

            LabEntry entry = new LabEntry();
            lab.LabEntry = entry;
            entry.LabWork = lab;
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

        //В id передается результат запроса
        [HttpPost]
        public string EditLab(long Id)
        {
            CreateLabModel res = new CreateLabModel();
            LabWork lab;
            try
            {
                lab = _ctx.LabWorks.Find(Id);
            }
            catch (Exception)
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

        public ActionResult CreateVariant(long labId = 0)
        {
            this.AllowAnonymous(_ctx);

            var lab = _ctx.LabWorks.Find(labId);

            CreateLabVariantModel model = new CreateLabVariantModel();
            model.id = lab.Id;
            model.Name = lab.Name;
            model.Variant = new Dictionary<string, List<KeyValuePair<long, string>>>();

            foreach (var t in lab.LabEntry.Tasks)
            {
                var list = new List<KeyValuePair<long, string>>();
                foreach (var v in t.TaskVariants)
                {
                    list.Add(new KeyValuePair<long, string>(v.Id, v.Number));
                }
                model.Variant.Add(t.Name, list);
            }

            return View(model);
        }

        [HttpPost]
        public string CreateVariant(int Id, string Number, string JsonArr)
        {
            this.AllowAnonymous(_ctx);

            int[] varId = JsonConvert.DeserializeObject<int[]>(JsonArr);

            int result = 0;

            var lab = _ctx.LabWorks.Find(Id);
            if (lab == null)
            {
                result = 1;
            }

            LabVariant labVar = new LabVariant();
            labVar.LabWork = lab;
            labVar.Number = Number;
            for (int i = 0; i < varId.Length; ++i)
            {
                labVar.TaskVariants.Add(_ctx.TaskVariants.Find(varId[i]));
            }

            try
            {
                _ctx.LabVariants.Add(labVar);
                _ctx.SaveChanges();
            }
            catch (Exception)
            {
                result = 2;
            }

            return JsonConvert.SerializeObject(result);
        }
    }
}
