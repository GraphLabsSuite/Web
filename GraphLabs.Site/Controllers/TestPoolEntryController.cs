using System;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.TestPoolEntry;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
    public class TestPoolEntryController : GraphLabsController
    {
        private readonly IEntityBasedModelSaver<SaveTestPoolEntryModel, TestPoolEntry> _modelSaver;
        private readonly IEntityRemover<TestPoolEntry> _modelRemover;
        private readonly IEntityBasedModelLoader<TestPoolEntryModel, TestPoolEntry> _modelLoader;

        public TestPoolEntryController(
            IEntityBasedModelSaver<SaveTestPoolEntryModel, TestPoolEntry> modelSaver,
            IEntityRemover<TestPoolEntry> modelRemover,
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
            try
            {
                var testPoolEntryCreated = _modelSaver.CreateOrUpdate(saveTestPoolEntry);
                return Json(testPoolEntryCreated.Id);
            }
            catch (EntityNotFoundException e)
            {
                return Json(false);
            }
            catch (GraphLabsDbUpdateException e)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public ActionResult Edit(AuxTestPoolEntryModel editTestPoolEntry)
        {
            try
            {
                var type = editTestPoolEntry.Type;
                var value = editTestPoolEntry.Value;
                var entity = _modelLoader.Load(editTestPoolEntry.Id);
               /* switch (type)
                {
                    case nameof(SaveTestPoolEntryModel.ScoringStrategy):
                        switch (value)
                        {
                            case 0:
                                entity.ScoringStrategy = ScoringStrategy.AllCorrectVariantsShouldBeSpecified;
                                break;
                            case 1:
                                entity.ScoringStrategy = ScoringStrategy.AnyCorrectVariantCanBeSpecified;
                                break;
                            default:
                                break;
                        }
                        break;
                    case nameof(SaveTestPoolEntryModel.Score):
                        entity.Score = value;
                        break;
                    default:
                        throw new Exception("Присланное не соответствует формату данных!");
                } */
                var model = new SaveTestPoolEntryModel
                {
                    Id = entity.Id,
                    SubCategoryId = entity.SubCategory.Id,
                    TestPool = entity.TestPool.Id
                };
                _modelSaver.CreateOrUpdate(model);
                return Json(true);
            }
            catch (GraphLabsDbUpdateException e)
            {
                return Json(false);
            }
            catch (EntityNotFoundException e)
            {
                return Json(false);
            }
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
            catch (GraphLabsDbUpdateException e)
            {
                return Json(false);
            }
            catch (EntityNotFoundException e)
            {
                return Json(false);
            }
        }

    }
}
