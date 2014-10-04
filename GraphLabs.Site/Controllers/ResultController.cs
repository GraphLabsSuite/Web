using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.DomainModel.Services;
using GraphLabs.Site.Models;
using Newtonsoft.Json;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Teacher | UserRole.Administrator)]
    public class ResultController : GraphLabsController
	{
		#region Зависимости

		private IGroupRepository _groupsRepository
		{
			get { return DependencyResolver.GetService<IGroupRepository>(); }
		}

		private ILabRepository _labsRepository
		{
			get { return DependencyResolver.GetService<ILabRepository>(); }
		}
		
        private ISystemDateService DateService
        {
            get { return DependencyResolver.GetService<ISystemDateService>(); }
        }

		#endregion

		public ActionResult Index()
        {
            ResultModel res = new ResultModel();

			res.Groups = _groupsRepository
							.GetAllGroups()
							.Select(g => new GroupModel(g, DateService))
							.ToArray();

			res.Labs = _labsRepository.GetLabWorks();
			
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

			result.LabName = _labsRepository.GetLabWorkById(Lab).Name;

            GroupModel group;
            int j = 0;

            foreach (long i in groupsId)
            {
                group = new GroupModel(_groupsRepository.GetGroupById(i), DateService);
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
            result.Name = (new GroupModel(_groupsRepository.GetGroupById(GroupId), DateService)).Name;

            return "";
        }
    }
}
