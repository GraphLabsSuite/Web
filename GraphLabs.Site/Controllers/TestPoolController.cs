using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Models;
using GraphLabs.Site.Models.Groups;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.TestPool;
using GraphLabs.Site.Models.TestPoolEntry;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
    public class TestPoolController : GraphLabsController
    {

        private readonly IListModelLoader _listModelLoader;
        private readonly IEntityBasedModelSaver<TestPoolModel, TestPool> _modelSaver;
        private readonly IEntityBasedModelLoader<TestPoolModel, TestPool> _modelLoader;
        private readonly IEntityBasedModelRemover<TestPoolModel, TestPool> _modelRemover;

        public TestPoolController(
            IListModelLoader listModelLoader,
            IEntityBasedModelSaver<TestPoolModel, TestPool> modelSaver,
            IEntityBasedModelLoader<TestPoolModel, TestPool> modelLoader,
            IEntityBasedModelRemover<TestPoolModel, TestPool> modelRemover
            )
        {
            _listModelLoader = listModelLoader;
            _modelSaver = modelSaver;
            _modelLoader = modelLoader;
            _modelRemover = modelRemover;
        }

        [HttpGet]
        public ActionResult Index(string message)
        {
            ViewBag.Message = message;

            var model = _listModelLoader.LoadListModel<TestPoolListModel, TestPoolModel>();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(long? testPoolId)
        {
            var testPool = _modelLoader.Load(testPoolId);
            TestPool res = _modelSaver.CreateOrUpdate(testPool);
            if (res != null) {
                return RedirectToAction("Index");
            }

            ViewBag.Message = "Невозможно сохранить тестпул";
            return View(testPool);
        }

        public ActionResult Edit(long id = 0)
        {
            return View(_modelLoader.Load(id));
        }

        [HttpPost]
        public ActionResult Edit(long? testPoolId)
        {
            var testPool = _modelLoader.Load(testPoolId);
            TestPool res = _modelSaver.CreateOrUpdate(testPool);
            if (res != null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Message = "Невозможно обновить тестпул";
            return View(testPool);
        }

        public ViewResult Delete(long? testPoolId)
        {
            var testPool = _modelLoader.Load(testPoolId);
            var result = _modelRemover.Remove(testPool);
            if (result == RemovalStatus.Success)
            {
                ViewBag.Message = "The test pool has been successfully deleted!";
                var model = _listModelLoader.LoadListModel<TestPoolListModel, TestPoolModel>();
                return View("Index", model);
            }
            else
            {
                ViewBag.Message = "There are still some foreign keys on this test pool!";
                var model = _listModelLoader.LoadListModel<TestPoolListModel, TestPoolModel>();
                return View("Index", model);
            }
        }

    }
}
