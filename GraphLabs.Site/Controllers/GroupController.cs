using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.DomainModel.Services;
using GraphLabs.Site.Utils;
using GraphLabs.Site.Models;
using GraphLabs.Site.Logic.GroupLogic;

namespace GraphLabs.Site.Controllers
{
    public class GroupController : Controller
    {
        private readonly GraphLabsContext _ctx = new GraphLabsContext();
        private GroupLogic logic = new GroupLogic();

        #region Формирование списка групп
        public ActionResult Index(string message)
        {
            if (!this.IsUserInRole(_ctx, UserRole.Teacher))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }

            Group[] groups = logic.GetGroupsFromDB();
            GroupModel[] groupModel = groups.Select(t => new GroupModel(t)).ToArray();

            return View(groupModel);
        }
        #endregion

        #region Создание группы
        public ActionResult Create()
        {
            if (!this.IsUserInRole(_ctx, UserRole.Teacher))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }

            Group group = new Group();
            group.FirstYear = DateTime.Today.Year;

            return View(group);
        }
        
        [HttpPost]
        public ActionResult Create(Group group)
        {
            if (!this.IsUserInRole(_ctx, UserRole.Teacher))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }

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
            if (!this.IsUserInRole(_ctx, UserRole.Teacher))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }

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
            if (!this.IsUserInRole(_ctx, UserRole.Teacher))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }

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