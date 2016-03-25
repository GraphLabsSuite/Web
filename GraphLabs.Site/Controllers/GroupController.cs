using GraphLabs.Site.Controllers.Attributes;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Groups;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
    public class GroupController : GraphLabsController
    {
        private readonly IListModelLoader _listModelLoader;
        private readonly IEntityBasedModelSaver<GroupModel, Group> _modelSaver;
        private readonly IEntityBasedModelLoader<GroupModel, Group> _modelLoader;

        public GroupController(
            IListModelLoader listModelLoader,
            IEntityBasedModelSaver<GroupModel, Group> modelSaver,
            IEntityBasedModelLoader<GroupModel, Group> modelLoader)
        {
            _listModelLoader = listModelLoader;
            _modelSaver = modelSaver;
            _modelLoader = modelLoader;
        }

        public ActionResult Index(string message)
        {
            ViewBag.Message = message;

            var model = _listModelLoader.LoadListModel<GroupListModel, GroupModel>();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Create(GroupModel group)
        {
            if (ModelState.IsValid)
            {
                _modelSaver.CreateOrUpdate(group);
                return RedirectToAction("Index");
            }

            ViewBag.Message = "Невозможно сохранить группу";
            return View(group);
        }

        public ActionResult Edit(long id = 0)
        {
            return View(_modelLoader.Load(id));
        }

        [HttpPost]
        public ActionResult Edit(GroupModel group)
        {
            if (ModelState.IsValid)
            {
                _modelSaver.CreateOrUpdate(group);
                return RedirectToAction("Index");
            }

            ViewBag.Message = "Невозможно обновить группу";
            return View(group);
        }
    }
}