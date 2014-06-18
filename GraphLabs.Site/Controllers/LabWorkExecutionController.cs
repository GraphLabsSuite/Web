using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Logic;
using GraphLabs.Site.Models;
using System;
using System.Web.Mvc;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher, UserRole.Student)]
    public class LabWorkExecutionController : GraphLabsController
    {
        private const string LAB_VARIABLE_KEY = "LabWork";

        #region Зависимости

        private ILabRepository LabRepository
        {
            get { return DependencyResolver.GetService<ILabRepository>(); }
        }

        private IAuthenticationSavingService AuthSavingService
        {
            get { return DependencyResolver.GetService<IAuthenticationSavingService>(); }
        }

        private IResultsManager ResultsManager
        {
            get { return DependencyResolver.GetService<IResultsManager>(); }
        }

        #endregion

        public ActionResult Index(long labId, long labVarId)
        {
            #region Проверки корректности GET запроса

            if (!LabRepository.CheckLabWorkExist(labId))
            {
                ViewBag.Message = "Запрашиваемая лабораторная работа не существует";
                return View("LabWorkExecutionError");
            }

            if (!LabRepository.CheckLabVariantExist(labVarId))
            {
                ViewBag.Message = "Запрашиваемый вариант лабораторной работы не существует";
                return View("LabWorkExecutionError");
            }

            if (!LabRepository.CheckLabVariantBelongLabWork(labId, labVarId))
            {
                ViewBag.Message = "Запрашиваемый вариант принадлежит другой лабораторной работе";
                return View("LabWorkExecutionError");
            }

            if (!LabRepository.VerifyCompleteVariant(labVarId))
            {
                ViewBag.Message = "Вариант лабораторной работы не завершен";
                return View("LabWorkExecutionError");
            }

            #endregion

            var session = GetSessionGuid();
            ResultsManager.StartLabExecution(labVarId, session);
            LabWork lab = LabRepository.GetLabWorkById(labId);
            TaskVariant[] variants = LabRepository.GetTaskVariantsByLabVarId(labVarId);
            var labWork = new LabWorkExecutionModel(session, lab, variants);

            labWork.SetNotSolvedTaskToCurrent();
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
            try
            {
                model.SetCurrentTask(Task);
            }
            catch (Exception)
            {
                ViewBag.Message = "Выбранное задание уже выполнено";
                return View("LabWorkExecutionError");
            }
            Session[LAB_VARIABLE_KEY] = model;
            return View("Index", model);
        }

        public ActionResult TaskComplete()
        {
            var model = (LabWorkExecutionModel)Session[LAB_VARIABLE_KEY];

            model.SetCurrentTaskToComplete();
            if (model.CheckCompleteLab())
            {
                ViewBag.Message = "Лабораторная работа выполнена!";
                return View("LabWorkExecutionError");
            }
            model.SetNotSolvedTaskToCurrent();

            Session[LAB_VARIABLE_KEY] = model;
            return View("Index", model);
        }
    }
}
