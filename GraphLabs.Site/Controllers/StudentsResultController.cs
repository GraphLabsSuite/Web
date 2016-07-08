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
            var result = new JSONResultLabResultInfo();

            var labName = _ctx.LabVariants.Single(labvar => labvar.Id == id).LabWork.Name;
            var resultStudent = _ctx.Results.Where(tr => tr.Student.Id == _studentId).ToArray();
            var groupId = resultStudent[0].Student.Group.Id;
            result.Result = 0;
            result.LabName = labName;
            result.StudentsNumber = _ctx.Groups.Count(tr => tr.Id == groupId);
            result.Place = GetPlace(id);
            result.Tasks = GetTaskInfo(id);
            result.Problems = GetProblems(id);

            return result;
        }

        private TaskInfo[] GetTaskInfo(int lab)
        {
            var tasks = _ctx.TaskResults.Where(task => task.Result.Student.Id == _studentId && task.Result.LabVariant.Id == lab).ToArray();
            var result = new TaskInfo[tasks.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new TaskInfo(tasks,i);
            }
            return result;
        }

        private string[] GetProblems(int id)
        {
            string[] result;
            var problems =
                _ctx.StudentActions.Where(tr => tr.TaskResult.Result.LabVariant.Id == id && tr.TaskResult.Result.Student.Id == _studentId && tr.Penalty != 0)
                    .ToArray();
            if (problems.Length == 0)
            {
               result = new string[1];
                result[0] = "У вас нет проблемных зон.";
            }
            else
            {
                result = new string[problems.Length];
                for (int i = 0; i < problems.Length; i++)
                {
                    result[i] = problems[i].Description;
                }
            }
            return result;
        }

        private int GetPlace(int id)
        {
            var students = _ctx.Results.Where(tr => tr.LabVariant.Id == id).OrderBy(td => td.Score).ToArray();
            var place = 1;
            for (int i = 0; i < students.Length; i++)
            {
                if (students[i].Student.Id != _studentId) place++;
                else return place;
            }
            return place;
        }

        private StudentsResultModel[] BuildStudentResultModel(string studentMail)
        {
            var studentId = _ctx.Sessions.Single(tr => tr.User.Email == studentMail).User.Id;
            var results = _ctx.Results.Where(tr => tr.Student.Id == studentId && tr.Score != null).ToArray();
            var resultModel = new StudentsResultModel[results.Length];
            for (int i = 0; i < results.Length; i++)
            {
                var temp = new StudentsResultModel(results[i].LabVariant.LabWork.Id, results[i].LabVariant.LabWork.Name, results[i].StartDateTime, results[i].LabVariant.Number, results[i].LabVariant.Id, (int) results[i].Score);
                resultModel[i] = temp;
            }
            return resultModel;
        }

    }
}
