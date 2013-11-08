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

        public ActionResult Create()
        {
            this.AllowAnonymous(_ctx);

            //var tasks = (from g in _ctx.Tasks
            //            select g).ToArray();
            
            CreateLabModel model = new CreateLabModel();
            model.Tasks = new List<KeyValuePair<long, string>>();
            /*foreach (var t in tasks)
            {                
                model.Tasks.Add(new KeyValuePair<long, string>(1, t.Name));
            }*/

            model.Tasks.Add(new KeyValuePair<long, string>(1, "qqq"));

            return View(model);
        }

        [HttpPost]
        public string Create(string Name, string DateFrom, string DateTo, string JsonArr)
        {
            this.AllowAnonymous(_ctx);

            int[] tasksId = JsonConvert.DeserializeObject<int[]>(JsonArr);
            
            /*var existlab = (from l in _ctx.LabWorks
                            where l.Name == Name
                            select l).ToList();
            */
            JSONResultCreateLab res = null;
            /*
            if (existlab.Count != 0)
            {
                res = new JSONResultCreateLab { Result = 1, LabName = Name };
                return JsonConvert.SerializeObject(res);
            };
            
            LabWork lab = new LabWork();
            lab.Name = Name;
            lab.AcquaintanceFrom = JsonConvert.DeserializeObject<DateTime>(DateFrom);
            lab.AcquaintanceTill = JsonConvert.DeserializeObject<DateTime>(DateTo);
            _ctx.LabWorks.Add(lab);
            _ctx.SaveChanges();
            
            LabEntry entry = new LabEntry();
            entry.LabWork = lab;
            foreach (var t in tasksId)
            {
                entry.Tasks.Add(_ctx.Tasks.Find(t));
            };
            _ctx.LabEntries.Add(entry);
            _ctx.SaveChanges();
            */
            res = new JSONResultCreateLab { Result = 0, LabId = 17, LabName = Name };
            return JsonConvert.SerializeObject(res);
        }

        public ActionResult CreateVariant(long labId = 0)
        {
            this.AllowAnonymous(_ctx);

            CreateLabVariantModel model = new CreateLabVariantModel();
            model.id = 1;
            model.Name = "Название";
            model.Variant = new Dictionary<string, List<KeyValuePair<long, string>>>();
            model.Variant.Add("задание 1", new List<KeyValuePair<long, string>> { new KeyValuePair<long, string>(1, "вариант 1"), new KeyValuePair<long, string>(2, "вариант 2"), new KeyValuePair<long, string>(3, "вариант 3") });
            model.Variant.Add("задание 2", new List<KeyValuePair<long, string>> { new KeyValuePair<long, string>(1, "вариант 1"), new KeyValuePair<long, string>(2, "вариант 2") });
            model.Variant.Add("задание 3", new List<KeyValuePair<long, string>> { new KeyValuePair<long, string>(1, "вариант 1"), new KeyValuePair<long, string>(2, "вариант 2"), new KeyValuePair<long, string>(3, "вариант 3") });
            model.Variant.Add("задание 4", new List<KeyValuePair<long, string>> { new KeyValuePair<long, string>(1, "вариант 1") });

            return View(model);
        }

    }
}
