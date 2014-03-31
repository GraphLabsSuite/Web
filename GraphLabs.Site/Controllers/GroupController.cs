using GraphLabs.DomainModel;
using GraphLabs.Site.Logic.GroupLogic;
using GraphLabs.Site.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
    public class GroupController : GraphLabsController
    {
        private GroupLogic logic = new GroupLogic();

        #region Формирование списка групп

        public ActionResult Index(string message)
        {
            Group[] groups = logic.GetGroupsFromDB();
            GroupModel[] groupModel = groups.Select(t => new GroupModel(t)).ToArray();

            return View(groupModel);
        }
        #endregion

        #region Создание группы
        public ActionResult Create()
        {
            Group group = new Group();
            group.FirstYear = DateTime.Today.Year;

            return View(group);
        }
        
        [HttpPost]
        public ActionResult Create(Group group)
        {
            if (ModelState.IsValid)
            {
                logic.SaveGroupToDB(group);
                return RedirectToAction("Index");
            }

            return View(group);
        }
        #endregion

        #region Редактирование группы
        public ActionResult Edit(long id = 0)
        {
            Group group = logic.GetGroupByID(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            GroupModel gr = new GroupModel(group);

            return View(gr);
        }

        [HttpPost]
        public ActionResult Edit(GroupModel gr)
        {
            if (ModelState.IsValid)
            {
                Group group = logic.GetGroupByID(gr.Id);
                if (group == null)
                {
                    return HttpNotFound();
                }
                logic.ModifyGroupInDB(group, gr.Number, gr.FirstYear, gr.IsOpen);
                return RedirectToAction("Index");
            }
            return View(gr);
        }
        #endregion
    }
}