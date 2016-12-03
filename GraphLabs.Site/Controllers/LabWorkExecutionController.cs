using System.Linq;
using GraphLabs.Site.Controllers.Attributes;
using System;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.LabExecution;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Student)]
    public class LabWorkExecutionController : GraphLabsController
    {
        #region Зависимости

        private readonly IDemoVariantModelLoader _demoVariantModelLoader;

        #endregion

        public LabWorkExecutionController(IDemoVariantModelLoader demoVariantModelLoader)
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

        public ActionResult Index(long labVarId, int? taskIndex = null)
        {
            var nextTaskLink = GetNextTaskUri(labVarId);
            return View(_demoVariantModelLoader.Load(labVarId, taskIndex, nextTaskLink));
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

        public ActionResult Index(long labVarId, int? taskIndex = null)
        {
            var nextTaskLink = GetNextTaskUri(labVarId);
            return View(_demoVariantModelLoader.Load(labVarId, taskIndex, nextTaskLink));
        }
    }
}
