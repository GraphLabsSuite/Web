using System.Linq;
using GraphLabs.Site.Controllers.Attributes;
using System;
using System.Net;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.LabExecution;
using GraphLabs.Site.Models.StudentAnswer;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Student)]
    public class LabWorkExecutionController : GraphLabsController
    {
        #region Зависимости

        private readonly IDemoVariantModelLoader _demoVariantModelLoader;
        private readonly IEntityBasedModelSaver<StudentAnswerModel, StudentAnswer> _answerSaver;

        #endregion

        public LabWorkExecutionController(
            IDemoVariantModelLoader demoVariantModelLoader,
            IEntityBasedModelSaver<StudentAnswerModel,StudentAnswer> answerSaver 
            )
        {
            _demoVariantModelLoader = demoVariantModelLoader;
            _answerSaver = answerSaver;
        }

        private Uri GetNextTaskUri(long labVarId)
        {
            return new Uri(Url.Action(
                nameof(Index),
                (string)RouteData.Values["controller"],
                new { LabVarId = labVarId },
                Request.Url.Scheme
                ));
        }

        public ActionResult Index(long labVarId, int? taskIndex = null, int? testIndex = null)
        {
            var nextTaskLink = GetNextTaskUri(labVarId);
            return View(_demoVariantModelLoader.Load(labVarId, taskIndex, testIndex, nextTaskLink));
        }

        /// <summary>
        /// Обработка выполнения тестового задания студентом
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Test(StudentAnswersModel answers)
        {
            try
            {
                foreach (var answer in answers.ChosenAnswerIds)
                {
                    _answerSaver.CreateOrUpdate(new StudentAnswerModel
                    {
                        ChosenAnswerId = answer,
                        TestResultId = answers.TestResultId
                    });
                }
                return Json(true);
            }
            catch (GraphLabsDbUpdateException e)
            {
                return Json(false);
            }
        }
    }

    [GLAuthorize(UserRole.Student)]
    public class TestingLabWorkExecutionController : GraphLabsController
    {
        #region Зависимости

        private readonly IDemoVariantModelLoader _demoVariantModelLoader;

        #endregion

        public TestingLabWorkExecutionController(IDemoVariantModelLoader demoVariantModelLoader)
        {
            _demoVariantModelLoader = demoVariantModelLoader;
        }

        private Uri GetNextTaskUri(long labVarId)
        {
            return new Uri(Url.Action(
                nameof(Index),
                (string)RouteData.Values["controller"],
                new { LabVarId = labVarId },
                Request.Url.Scheme
                ));
        }

        public ActionResult Index(long labVarId, int? testIndex, int? taskIndex = null)
        {
            var nextTaskLink = GetNextTaskUri(labVarId);
            return View(_demoVariantModelLoader.Load(labVarId, taskIndex, testIndex, nextTaskLink));
        }
    }
}
