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
        private long _studentId;
        private string _sessionId = System.Web.HttpContext.Current.Session.SessionID;
        //private readonly StudentsResultModel[] _model;

        public StudentsResultController(GraphLabsContext context)
        {
            _ctx = context;

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
            _studentId = _ctx.Users.Single(user => user.Email == studentMail).Id;
            var model = BuildStudentResultModel(studentMail);
            return View(model);
        }

        [HttpPost]
        public string GetLabDetail(int id)
        {
            var studentMail = HttpContext.User.Identity.Name;
            _studentId = _ctx.Users.Single(user => user.Email == studentMail).Id;
            var result = GetLabDetails(id);
            return JsonConvert.SerializeObject(result);
        }

        private JSONResultLabResultInfo GetLabDetails(int id)
        {
            var result = new JSONResultLabResultInfo(_ctx, id, _studentId);
            return result;
        }

        private StudentsResultModel[] BuildStudentResultModel(string studentMail)
        {
            var studentId = _ctx.Sessions.Single(tr => tr.User.Email == studentMail).User.Id;
            var results = _ctx.Results.Where(tr => tr.Student.Id == studentId).ToArray();
            var resultModel = new StudentsResultModel[results.Length];
            for (int i = 0; i < results.Length; i++)
            {
                var temp = new StudentsResultModel(results[i].LabVariant.LabWork.Id, results[i].LabVariant.LabWork.Name, results[i].StartDateTime, results[i].LabVariant.Number, results[i].LabVariant.Id, results[i].Score, results[i].Status);
                resultModel[i] = temp;
            }
            return resultModel;
        }

    }
}
