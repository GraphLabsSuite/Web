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

        public TestPoolController(
            IListModelLoader listModelLoader,
            IEntityBasedModelSaver<TestPoolModel, TestPool> modelSaver,
            IEntityBasedModelLoader<TestPoolModel, TestPool> modelLoader)
        {
            _listModelLoader = listModelLoader;
            _modelSaver = modelSaver;
            _modelLoader = modelLoader;
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
        public ActionResult Create(TestPoolModel testPool)
        {
            if (ModelState.IsValid)
            {
                _modelSaver.CreateOrUpdate(testPool);
                return RedirectToAction("Index");
            }

            ViewBag.Message = "Невозможно сохранить группу";
            return View(testPool);
        }

        public ActionResult Edit(long id = 0)
        {
            return View(_modelLoader.Load(id));
        }

        public ViewResult BlankEditorRow(TestPoolModel testPool)
        {
            var testPoolEntry = new TestPoolEntryModel();
            testPool.TestPoolEntries.Add(testPoolEntry);
            return View(
                "_TestPoolEntryRow",
                testPoolEntry
                );
        }

        [HttpPost]
        public ActionResult Edit(TestPoolModel testPool)
        {
            if (ModelState.IsValid)
            {
                _modelSaver.CreateOrUpdate(testPool);
                return RedirectToAction("Index");
            }

            ViewBag.Message = "Невозможно обновить тестпул";
            return View(testPool);
        }

    }
}
