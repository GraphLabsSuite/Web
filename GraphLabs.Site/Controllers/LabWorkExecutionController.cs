using GraphLabs.DomainModel;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Logic;
using GraphLabs.Site.Logic.Labs;
using GraphLabs.Site.Models;
using System.Web.Mvc;
using System;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher, UserRole.Student)]
    public class LabWorkExecutionController : GraphLabsController
    {
        private const string LAB_VARIABLE_KEY = "LabWork";

        private ILabRepository LabRepository
        {
            get { return DependencyResolver.GetService<ILabRepository>(); }
        }

        private ILabExecutionEngine LabExecutionEngine
        {
            get { return DependencyResolver.GetService<ILabExecutionEngine>(); }
        }

        private IAuthenticationSavingService AuthSavingService
        {
            get { return DependencyResolver.GetService<IAuthenticationSavingService>(); }
        }

        private IResultsManager ResultsManager
        {
            get { return DependencyResolver.GetService<IResultsManager>(); }
        }

        public ActionResult Index(long labId, long labVarId)
        {
            if (!LabExecutionEngine.IsLabVariantCorrect(labVarId))
            {
                ViewBag.Message = "Вариант лабораторной работы не завершен";
                return View("LabWorkExecutionError");
            }

            var session = GetSessionGuid();
            ResultsManager.StartLabExecution(labVarId, session);

            var labName = LabExecutionEngine.GetLabName(labId);
            var variants = LabRepository.GetTaskVariantsByLabVarId(labVarId);
            var labWork = new LabWorkExecutionModel(session, labName, labId, variants);

            labWork.SetCurrent(0);
            Session[LAB_VARIABLE_KEY] = labWork;
            return View(labWork);
        }

        private Guid GetSessionGuid()
        {
            var sessionInfo = AuthSavingService.GetSessionInfo();
            return sessionInfo.SessionGuid;
        }

        public ActionResult ChangeTask(int Task)
        {
            var model = (LabWorkExecutionModel)Session[LAB_VARIABLE_KEY];
            model.SetCurrent(Task);
            Session[LAB_VARIABLE_KEY] = model;
            return View("Index", model);
        }
    }
}
