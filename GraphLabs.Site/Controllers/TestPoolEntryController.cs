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
        private readonly IEntityBasedModelSaver<SaveTestPoolEntryModel, TestPoolEntry> _modelSaver;
        private readonly IEntityBasedModelRemover<TestPoolEntryModel, TestPoolEntry> _modelRemover;
        private readonly IEntityBasedModelLoader<TestPoolEntryModel, TestPoolEntry> _modelLoader;

        public TestPoolEntryController(
            IEntityBasedModelSaver<SaveTestPoolEntryModel, TestPoolEntry> modelSaver,
            IEntityBasedModelRemover<TestPoolEntryModel, TestPoolEntry> modelRemover,
            IEntityBasedModelLoader<TestPoolEntryModel, TestPoolEntry> modelLoader
            )
        {
            _modelRemover = modelRemover;
            _modelSaver = modelSaver;
            _modelLoader = modelLoader;
        }

        public ActionResult Index(string message)
        {
            ViewBag.Message = message;
            return RedirectToAction("Index", "TestPoolEntry");
        }

        [HttpPost]
        public ActionResult Create(SaveTestPoolEntryModel saveTestPoolEntry)
        {
            var testPoolEntryCreated = _modelSaver.CreateOrUpdate(saveTestPoolEntry);
            return Json(testPoolEntryCreated.Id);
        }

        [HttpPost]
        public ActionResult Edit(SaveTestPoolEntryModel saveTestPoolEntry)
        {
            _modelSaver.CreateOrUpdate(saveTestPoolEntry);
            return Json(true);
        }

        [HttpPost]
        public ActionResult Delete(SaveTestPoolEntryModel testPoolEntryIdStr)
        {
            try
            {
                var testPoolEntryId = testPoolEntryIdStr.Id;
                _modelRemover.Remove(testPoolEntryId);
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(false);
            }
        }

    }
}
