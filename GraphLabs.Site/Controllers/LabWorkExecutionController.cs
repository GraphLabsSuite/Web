using GraphLabs.DomainModel;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Logic.Labs;
using GraphLabs.Site.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher, UserRole.Student)]
    public class LabWorkExecutionController : GraphLabsController
    {
        private ILabExecutionEngine LabExecutionEngine
        {
            get { return DependencyResolver.GetService<ILabExecutionEngine>(); }
        }

        public ActionResult Index(long labId, long labVarId)
        {
            string labName = LabExecutionEngine.GetLabName(labId);
            if (labName == "")
            {
                return View("LabWorkNotFound");
            }
            var labWork = new LabWorkExecutionModel(labName);

            return View(labWork);
        }
    }
}
