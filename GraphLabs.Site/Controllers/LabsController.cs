﻿using System;
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
            result.LabId = Id;
            result.LabName = lab.Name;

            result.Tasks = new List<KeyValuePair<int, string>>();
            result.Tasks.Add(new KeyValuePair<int, string>(1, "Задание 1"));
            result.Tasks.Add(new KeyValuePair<int, string>(2, "Задание 2"));
            result.Tasks.Add(new KeyValuePair<int, string>(3, "Задание 3"));
            result.Tasks.Add(new KeyValuePair<int, string>(4, "Задание 4"));
            result.Variants = new JSONResultVariants[3];
            result.Variants[0] = new JSONResultVariants();
            result.Variants[0].VarId = 1;
            result.Variants[0].VarName = "Вариант 1";
            result.Variants[0].TasksVar = new List<KeyValuePair<int, string>>();
            result.Variants[0].TasksVar.Add(new KeyValuePair<int, string>(1, "Вариант 1"));
            result.Variants[0].TasksVar.Add(new KeyValuePair<int, string>(3, "Вариант 1"));
            result.Variants[1] = new JSONResultVariants();
            result.Variants[1].VarId = 2;
            result.Variants[1].VarName = "Вариант 2";
            result.Variants[1].TasksVar = new List<KeyValuePair<int, string>>();
            result.Variants[1].TasksVar.Add(new KeyValuePair<int, string>(1, "Вариант 2"));
            result.Variants[1].TasksVar.Add(new KeyValuePair<int, string>(2, "Вариант 2"));
            result.Variants[1].TasksVar.Add(new KeyValuePair<int, string>(3, "Вариант 2"));
            result.Variants[2] = new JSONResultVariants();
            result.Variants[2].VarId = 3;
            result.Variants[2].VarName = "Вариант 3";
            result.Variants[2].TasksVar = new List<KeyValuePair<int, string>>();
            result.Variants[2].TasksVar.Add(new KeyValuePair<int, string>(3, "Вариант 3"));

            return JsonConvert.SerializeObject(result);
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
                model.Tasks.Add(new KeyValuePair<long, string>(1, t.Name));
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
            };
            if (DateTo != "")
            {
                lab.AcquaintanceTill = DateTime.Parse(DateTo);
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
            model.Variant.Add("задание 1", new List<KeyValuePair<long, string>> { new KeyValuePair<long, string>(1, "вариант 1"), new KeyValuePair<long, string>(2, "вариант 2"), new KeyValuePair<long, string>(3, "вариант 3") });
            model.Variant.Add("задание 2", new List<KeyValuePair<long, string>> { new KeyValuePair<long, string>(1, "вариант 1"), new KeyValuePair<long, string>(2, "вариант 2") });
            model.Variant.Add("задание 3", new List<KeyValuePair<long, string>> { new KeyValuePair<long, string>(1, "вариант 1"), new KeyValuePair<long, string>(2, "вариант 2"), new KeyValuePair<long, string>(3, "вариант 3") });
            model.Variant.Add("задание 4", new List<KeyValuePair<long, string>> { new KeyValuePair<long, string>(1, "вариант 1") });

            return View(model);
        }

        [HttpPost]
        public string CreateVariant(int Id, string JsonArr)
        {
            this.AllowAnonymous(_ctx);

            int[] varId = JsonConvert.DeserializeObject<int[]>(JsonArr);

            int result = 0;

            var lab = _ctx.LabWorks.Find(Id);
            if (lab == null)
            {
                result = 1;
            }
            
            return JsonConvert.SerializeObject(result);
        }
    }
}
