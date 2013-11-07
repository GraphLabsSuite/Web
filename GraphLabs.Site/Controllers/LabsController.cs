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
            int[] tasksId = JsonConvert.DeserializeObject<int[]>(JsonArr);

            var existlab = (from l in _ctx.LabWorks
                            where l.Name == Name
                            select l).First();
            if (existlab != null)
            {
                return "fail";
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

            return "success";
        }

    }
}
