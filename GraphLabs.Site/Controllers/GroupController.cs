using GraphLabs.Dal.Ef.Services;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Models;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
    public class GroupController : GraphLabsController
    {
        #region Зависимости

        private readonly IGraphLabsContext _context;

        private readonly ISystemDateService _dateService;

        #endregion

        public GroupController(
            IGraphLabsContext context,
            ISystemDateService dateService)
        {
            _context = context;
            _dateService = dateService;
        }

        #region Формирование списка групп

        public ActionResult Index(string message)
        {
            return View(new GroupListModel(_context, _dateService).GetGroupList());
        }

        #endregion

        #region Создание группы

        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Create(GroupModel group)
        {
            if (ModelState.IsValid)
            {
                new GroupListModel(_context, _dateService).CreateNew(group);
                return RedirectToAction("Index");
            }

            ViewBag.Message = "Невозможно сохранить группу";
            return View(group);
        }

        #endregion

        #region Редактирование группы

        public ActionResult Edit(long id = 0)
        {
            GroupModel group = new GroupListModel(_context, _dateService).GetById(id);

            return View(group);
        }

        [HttpPost]
        public ActionResult Edit(GroupModel group)
        {
            if (ModelState.IsValid)
            {
                new GroupListModel(_context, _dateService).Edit(group);
                return RedirectToAction("Index");
            }
            ViewBag.Message = "Невозможно обновить группу";
            return View(group);
        }

        #endregion
    }
}