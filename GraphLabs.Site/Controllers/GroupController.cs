using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.DomainModel.Services;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Models;
using System.Linq;
using System.Web.Mvc;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
    public class GroupController : GraphLabsController
    {
        #region Зависимости

        private IGroupRepository GroupsRepository
        {
            get { return DependencyResolver.GetService<IGroupRepository>(); }
        }

        private ISystemDateService DateService
        {
            get { return DependencyResolver.GetService<ISystemDateService>(); }
        }

        #endregion

        #region Формирование списка групп

        public ActionResult Index(string message)
        {
            Group[] groups = GroupsRepository.GetAllGroups();
            GroupModel[] groupModel = groups.Select(t => new GroupModel(t, DateService)).ToArray();

            return View(groupModel);
        }

        #endregion

        #region Создание группы

        public ActionResult Create()
        {
            Group group = new Group();
            group.FirstYear = DateService.GetDate().Year;

            return View(group);
        }
        
        [HttpPost]
        public ActionResult Create(Group group)
        {
            if (ModelState.IsValid)
            {
                if (GroupsRepository.TrySaveGroup(group))
                {
                    return RedirectToAction("Index");
                }
                ViewBag.Message = "Невозможно сохранить группу";
            }

            return View(group);
        }

        #endregion

        #region Редактирование группы

        public ActionResult Edit(long id = 0)
        {
            GroupModel group = new GroupModel( GroupsRepository.GetGroupById(id), DateService );

            return View(group);
        }

        [HttpPost]
        public ActionResult Edit(GroupModel gr)
        {
            if (ModelState.IsValid)
            {
                if (GroupsRepository.TryModifyGroup(gr.Id, gr.Number, gr.FirstYear, gr.IsOpen))
                {
                    return RedirectToAction("Index");
                }

                ViewBag.Message = "Невозможно обновить группу";
            }
            return View(gr);
        }

        #endregion
    }
}