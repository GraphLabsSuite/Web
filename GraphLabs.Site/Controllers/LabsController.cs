using System;
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

        public ActionResult Create()
        {
            this.AllowAnonymous(_ctx);

            var tasks = (from g in _ctx.Tasks
                        select g).ToArray();
            
            CreateLabModel model = new CreateLabModel();
            model.Tasks = new List<KeyValuePair<long, string>>();
            foreach (var t in tasks)
            {                
                model.Tasks.Add(new KeyValuePair<long, string>(t.Id, t.Name));
            }

            return View(model);
        }

        [HttpPost]
        public string Create(string Name, string DateFrom, string DateTo, string JsonArr)
        {
            this.AllowAnonymous(_ctx);

            int[] tasksId = JsonConvert.DeserializeObject<int[]>(JsonArr);
            
            var existlab = (from l in _ctx.LabWorks
                            where l.Name == Name
                            select l).ToList();
            
            JSONResultCreateLab res = null;
            
            if (existlab.Count != 0)
            {
                res = new JSONResultCreateLab { Result = 1, LabName = Name };
                return JsonConvert.SerializeObject(res);
            };
            
            LabWork lab = new LabWork();
            lab.Name = Name;
            if (DateFrom != "")
            {
                lab.AcquaintanceFrom = DateTime.Parse(DateFrom);
            }
            else
            {
                lab.AcquaintanceFrom = new DateTime(2000, 01, 01);
            };
            if (DateTo != "")
            {
                lab.AcquaintanceTill = DateTime.Parse(DateTo);
            }
            else
            {
                lab.AcquaintanceTill = new DateTime(2000, 01, 01);
            };
            LabEntry entry = new LabEntry();
            lab.LabEntry = entry;
            entry.LabWork = lab;
            foreach (var t in tasksId)
            {
                entry.Tasks.Add(_ctx.Tasks.Find(t));
            };
            _ctx.LabWorks.Add(lab);
            _ctx.LabEntries.Add(entry);
            _ctx.SaveChanges();
            res = new JSONResultCreateLab { Result = 0, LabId = lab.Id, LabName = lab.Name };
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
