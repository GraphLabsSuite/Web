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
    public class TestPoolEntryController : GraphLabsController
    {


        private readonly IEntityBasedModelSaver<TestPoolEntryModel, TestPoolEntry> _modelSaver;
        private readonly IEntityBasedModelRemover<TestPoolEntryModel, TestPoolEntry> _modelRemover;
        private readonly IEntityBasedModelLoader<TestPoolEntryModel, TestPoolEntry> _modelLoader;
        private readonly IEntityBasedModelLoader<TestPoolModel, TestPool> _parentModelLoader;


        public TestPoolEntryController(
            IEntityBasedModelSaver<TestPoolEntryModel, TestPoolEntry> modelSaver,
            IEntityBasedModelRemover<TestPoolEntryModel, TestPoolEntry> modelRemover,
            IEntityBasedModelLoader<TestPoolEntryModel, TestPoolEntry> modelLoader,
            IEntityBasedModelLoader<TestPoolModel, TestPool> parentModelLoader
            )
        {
            _modelRemover = modelRemover;
            _modelSaver = modelSaver;
            _modelLoader = modelLoader;
            _parentModelLoader = parentModelLoader;
        }

        public ActionResult Index(string message)
        {
            ViewBag.Message = message;
            return RedirectToAction("Index", "TestPool");
        }

        public ViewResult Index(long? testPoolId)
        {
            var testPoolEntry = new TestPoolEntryModel();
            var testPool = _parentModelLoader.Load(testPoolId);
            testPool.TestPoolEntries.Add(testPoolEntry);
            return View(
                "_TestPoolEntryRow",
                testPoolEntry
                );
        }

        public ViewResult Create(long? testPoolId)
        {
            var testPoolEntry = new TestPoolEntryModel();
            var testPool = _parentModelLoader.Load(testPoolId);
            testPool.TestPoolEntries.Add(testPoolEntry);
            return View(
                "_TestPoolEntryRow",
                testPoolEntry
                );
        }

        [HttpPost]
        public ActionResult Edit(long? testPoolId)
        {
            var testPool = _modelLoader.Load(testPoolId);
            //TestPool res = _modelSaver.CreateOrUpdate(testPool);
            //if (res != null)
            {
                return RedirectToAction("Index", "TestPool");
            }
            ViewBag.Message = "Невозможно обновить тестпул";
            return RedirectToAction("Index", "TestPool");
        }

        public ActionResult Delete(long? testPoolEntryId)
        {
            var testPoolEntry = _modelLoader.Load(testPoolEntryId);
            TestPoolEntry elem = _modelSaver.CreateOrUpdate(testPoolEntry);
            elem.TestPool.TestPoolEntries.Remove(elem);
            _modelSaver.CreateOrUpdate(_modelLoader.Load(elem.TestPool));
            _modelRemover.Remove(testPoolEntry);
            return RedirectToAction("Index", "TestPool");
        }

    }
}
