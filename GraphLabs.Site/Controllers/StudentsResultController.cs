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
    public class StudentsResultController : Controller
    {
        private readonly GraphLabsContext _ctx = new GraphLabsContext();

        public ActionResult Index()
        {
            this.AllowAnonymous(_ctx);

            StudentsResultModel[] res = new StudentsResultModel[3];
            res[0] = new StudentsResultModel { Id = 1, Name = "Лаб 1", Date = new DateTime(2014, 3, 11), Variant = "1", Result = 90 };
            res[1] = new StudentsResultModel { Id = 2, Name = "Лаб 2", Date = new DateTime(2014, 3, 12), Variant = "2", Result = 70 };
            res[2] = new StudentsResultModel { Id = 3, Name = "Лаб 3", Date = new DateTime(2014, 3, 13), Variant = "1", Result = 95 };
            return View(res);
        }

        [HttpPost]
        public string GetLabDetail(int Id)
        {
            JSONResultLabResultInfo result = new JSONResultLabResultInfo();

            result.Result = 0;
            switch (Id)
            {
                case 1:
                    result.LabName = "Лаб 1";
                    break;
                case 2:
                    result.LabName = "Лаб 2";
                    break;
                default:
                    result.LabName = "Лаб 3";
                    break;
            }
            result.StudentsNumber = 15;
            result.Place = 3;
            result.Tasks = new TaskInfo[5];
            result.Tasks[0] = new TaskInfo { Id = 1, Name = "Задание 1", Variant = "3", Result = 90 };
            result.Tasks[1] = new TaskInfo { Id = 2, Name = "Задание 2", Variant = "2", Result = 80 };
            result.Tasks[2] = new TaskInfo { Id = 3, Name = "Задание 3", Variant = "3", Result = 100 };
            result.Tasks[3] = new TaskInfo { Id = 4, Name = "Задание 4", Variant = "1", Result = 94 };
            result.Tasks[4] = new TaskInfo { Id = 5, Name = "Задание 5", Variant = "3", Result = 63 };
            
            result.Problems = new string[3];
            result.Problems[0] = "Проблема 1";
            result.Problems[1] = "Проблема 2";
            result.Problems[2] = "Проблема 3";
            return JsonConvert.SerializeObject(result);
        }

    }
}
