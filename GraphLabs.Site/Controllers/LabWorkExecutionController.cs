using System.Linq;
using GraphLabs.DomainModel.EF;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Logic;
using GraphLabs.Site.Models;
using System;
using System.Web.Mvc;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher, UserRole.Student)]
    public class LabWorkExecutionController : GraphLabsController
    {
        private const string LAB_VARIABLE_KEY = "LabWork";

        #region Зависимости

        private readonly ILabRepository _labRepository;
        private readonly IAuthenticationSavingService _authSavingService;
        private readonly IResultsManager _resultsManager;
        private readonly ITaskExecutionModelFactory _taskExecutionModelFactory;

        #endregion

        public LabWorkExecutionController(
            ILabRepository labRepository, 
            IAuthenticationSavingService authSavingService, 
            IResultsManager resultsManager, 
            ITaskExecutionModelFactory taskExecutionModelFactory)
        {
            _labRepository = labRepository;
            _authSavingService = authSavingService;
            _resultsManager = resultsManager;
            _taskExecutionModelFactory = taskExecutionModelFactory;
        }

        public ActionResult Index(long labId, long labVarId)
        {
            #region Проверки корректности GET запроса

            if (!_labRepository.CheckLabWorkExist(labId))
            {
                ViewBag.Message = "Запрашиваемая лабораторная работа не существует";
                return View("LabWorkExecutionError");
            }

            if (!_labRepository.CheckLabVariantExist(labVarId))
            {
                ViewBag.Message = "Запрашиваемый вариант лабораторной работы не существует";
                return View("LabWorkExecutionError");
            }

            if (!_labRepository.CheckLabVariantBelongLabWork(labId, labVarId))
            {
                ViewBag.Message = "Запрашиваемый вариант принадлежит другой лабораторной работе";
                return View("LabWorkExecutionError");
            }

            if (!_labRepository.VerifyCompleteVariant(labVarId))
            {
                ViewBag.Message = "Вариант лабораторной работы не завершен";
                return View("LabWorkExecutionError");
            }

            #endregion

            var session = GetSessionGuid();
            _resultsManager.StartLabExecution(labVarId, session);
            LabWork lab = _labRepository.GetLabWorkById(labId);
            TaskVariant[] variants = _labRepository.GetTaskVariantsByLabVarId(labVarId);
            var labWork = new LabWorkExecutionModel(session, lab, variants
                .Select(v => _taskExecutionModelFactory.CreateForDemoMode(
                    session,
                    v.Task.Name,
                    v.Task.Id,
                    v.Id,
                    lab.Id))
                .ToArray());

            labWork.SetNotSolvedTaskToCurrent();
            Session[LAB_VARIABLE_KEY] = labWork;
            return View(labWork);
        }

        private Guid GetSessionGuid()
        {
            var sessionInfo = _authSavingService.GetSessionInfo();
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
