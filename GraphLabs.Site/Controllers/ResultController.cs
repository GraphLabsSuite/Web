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
    public class ResultController : Controller
    {
        private readonly GraphLabsContext _ctx = new GraphLabsContext();

        public ActionResult Index()
        {
            this.AllowAnonymous(_ctx);

            ResultModel res = new ResultModel();

            res.Groups = (from g in _ctx.Groups
                          select g).ToArray()
                          .Select(t => new GroupModel(t))
                          .ToArray();

            res.Labs = (from l in _ctx.LabWorks
                        select l).ToArray();

            return View(res);
        }

        [HttpPost]
        public string GetGroupsInfo(string Groups, long Lab)
        {
            Groups = "[" + Groups + "]";
            List<long> groupsId = JsonConvert.DeserializeObject<List<long>>(Groups);

            JSONResultGroupsInfoModel result = new JSONResultGroupsInfoModel();
            result.Result = 0;
            result.Marks = new GroupResult[groupsId.Count];

            result.LabName = _ctx.LabWorks.Find(Lab).Name;

            GroupModel group;
            int j = 0;

            foreach (long i in groupsId)
            {
                group = new GroupModel(_ctx.Groups.Find(i));
                result.Marks[j] = new GroupResult();
                result.Marks[j].Id = group.Id;
                result.Marks[j].Name = group.Name;
                result.Marks[j].StudentsCount = group.Students.Count;
                Random r = new Random();
                int x = group.Students.Count;
                int y = r.Next(0, x+1);
                x = x-y;
                result.Marks[j].Count5 = y;
                y = r.Next(0, x + 1);
                x = x - y;
                result.Marks[j].Count4 = y;
                y = r.Next(0, x + 1);
                x = x - y;
                result.Marks[j].Count3 = y;
                y = r.Next(0, x + 1);
                x = x - y; 
                result.Marks[j].Count2 = y;
                result.Marks[j].Count0 = x;
                ++j;
            }

            return JsonConvert.SerializeObject(result);
        }

        [HttpPost]
        public string GetGroupDetail(long GroupId, long LabId)
        {
            JSONResultGroupDetail result = new JSONResultGroupDetail();
            result.Result = 0;
            result.Name = (new GroupModel(_ctx.Groups.Find(GroupId))).Name;

            return "";
        }
    }
}
