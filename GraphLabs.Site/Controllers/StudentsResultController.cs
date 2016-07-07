using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GraphLabs.Dal.Ef;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Models;
using GraphLabs.Site.Models.CreateLab;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.Lab;
using GraphLabs.Site.Utils;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Student)]
    public class StudentsResultController : GraphLabsController
    {
        private readonly GraphLabsContext _ctx;
 
        private string _sessionId = System.Web.HttpContext.Current.Session.SessionID;
        //private readonly StudentsResultModel[] _model;

        public StudentsResultController(GraphLabsContext context)
        {
            _ctx = context;
            //_studentGuid = new Guid("7568BF04-DAF7-48EE-88EA-748949C46F62");
        }

        public ActionResult Index()
        {
            //this.AllowAnonymous(); 
            //StudentsResultModel[] res = new StudentsResultModel[3];
            //res[0] = new StudentsResultModel (1, 2, "Лаб 1",new DateTime(2014, 3, 11), "1", 90);
            //res[1] = new StudentsResultModel (2, 2, "Лаб 2", new DateTime(2014, 3, 12), "2", 70);
            //res[2] = new StudentsResultModel (3, 2, "Лаб 3", new DateTime(2014, 3, 13), "1", 95); 
            //var studentGuid = new Guid(_sessionId);
            var studentMail = HttpContext.User.Identity.Name;
            var model = BuildStudentResultModel(studentMail);
            return View(model);
        }

        [HttpPost]
        public string GetLabDetail(int id)
        {
            var result = BuildLabDetails(id);
            return JsonConvert.SerializeObject(result);
        }

        private JSONResultLabResultInfo BuildLabDetails(int id)
        {
            var result = new JSONResultLabResultInfo();
            result.Result = 0;
            switch (id)
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
            return result;
        }

        private StudentsResultModel[] BuildStudentResultModel(string studentMail)
        {
            var studentId = _ctx.Sessions.Single(tr => tr.User.Email == studentMail).User.Id;
            var results = _ctx.Results.Where(tr => tr.Student.Id == studentId && tr.Score != null).ToArray();
            var resultModel = new StudentsResultModel[results.Length];
            for (int i = 0; i < results.Length; i++)
            {
                var temp = new StudentsResultModel(results[i].LabVariant.LabWork.Id, results[i].LabVariant.LabWork.Name, results[i].StartDateTime, results[i].LabVariant.Number, (int) results[i].Score);
                resultModel[i] = temp;
            }
            return resultModel;
        }

    }
}
