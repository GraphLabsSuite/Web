using System.Linq;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Logic;
using GraphLabs.Site.Models;
using System;
using System.Data.Entity;
using System.Runtime.Remoting.Messaging;
using System.Web.Mvc;
using GraphLabs.Dal.Ef;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.Site.Utils;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher, UserRole.Student)]
    public class LabWorkExecutionController : GraphLabsController
    {
        private const string LAB_VARIABLE_KEY = "LabWork";

        #region Зависимости

        private readonly IAuthenticationSavingService _authSavingService;
        private readonly IResultsManager _resultsManager;
        private readonly ITaskExecutionModelFactory _taskExecutionModelFactory;
        private readonly IEntityQuery _query;

        #endregion

        public LabWorkExecutionController(
            IEntityQuery query,
            IAuthenticationSavingService authSavingService, 
            IResultsManager resultsManager, 
            ITaskExecutionModelFactory taskExecutionModelFactory)
        {
            _query = query;
            _authSavingService = authSavingService;
            _resultsManager = resultsManager;
            _taskExecutionModelFactory = taskExecutionModelFactory;
        }

        private Uri GetNextTaskUri()
        {
            return new Uri(Url.Action(
                nameof(TaskComplete),
                (string)RouteData.Values["controller"],
                null,
                Request.Url.Scheme
                ));
        }

        //TODO: Придумать что-то с LabModel
        private bool CheckLabWorkExist(long labId)
        {
            return _query.OfEntities<LabWork>().SingleOrDefault(l => l.Id == labId) != null;
        }

        private bool CheckLabVariantExist(long labVarId)
        {
            return _query.OfEntities<LabVariant>().SingleOrDefault(l => l.Id == labVarId) != null;
        }

        private bool CheckLabVariantBelongLabWork(long labId, long labVarId)
        {
            return _query.OfEntities<LabVariant>().Where(lv => lv.Id == labVarId).SingleOrDefault(lv => lv.LabWork.Id == labId) != null;
        }

        public bool VerifyCompleteVariant(long variantId)
        {
            long labWorkId = _query.OfEntities<LabVariant>()
                .Where(v => v.Id == variantId)
                .Select(v => v.LabWork.Id)
                .Single();

            long[] labEntry = _query.OfEntities<LabEntry>()
                .Where(e => e.LabWork.Id == labWorkId)
                .Select(e => e.Task.Id)
                .ToArray();

            long[] currentVariantEntry = _query.OfEntities<LabVariant>()
                .Where(l => l.Id == variantId)
                .SelectMany(t => t.TaskVariants)
                .Select(t => t.Task.Id)
                .ToArray();

            return labEntry.ContainsSameSet(currentVariantEntry);
        }

        private LabWork GetLabWorkById(long labId)
        {
            return _query.OfEntities<LabWork>().SingleOrDefault(l => l.Id == labId);
        }

        private TaskVariant[] GetTaskVariantsByLabVarId(long labVarId)
        {
            return _query.OfEntities<LabVariant>()
                .Where(v => v.Id == labVarId)
                .SelectMany(v => v.TaskVariants)
                .Include(v => v.Task)
                .ToArray();
        }

        public ActionResult Index(long labId, long labVarId)
        {
            #region Проверки корректности GET запроса

            if (!CheckLabWorkExist(labId))
            {
                ViewBag.Message = "Запрашиваемая лабораторная работа не существует";
                return View("LabWorkExecutionError");
            }

            if (!CheckLabVariantExist(labVarId))
            {
                ViewBag.Message = "Запрашиваемый вариант лабораторной работы не существует";
                return View("LabWorkExecutionError");
            }

            if (!CheckLabVariantBelongLabWork(labId, labVarId))
            {
                ViewBag.Message = "Запрашиваемый вариант принадлежит другой лабораторной работе";
                return View("LabWorkExecutionError");
            }

            if (!VerifyCompleteVariant(labVarId))
            {
                ViewBag.Message = "Вариант лабораторной работы не завершен";
                return View("LabWorkExecutionError");
            }

            #endregion
            var session = GetSessionGuid();
            var nextTaskLink = GetNextTaskUri();
            _resultsManager.StartLabExecution(labVarId, session);
            LabWork lab = GetLabWorkById(labId);
            TaskVariant[] variants = GetTaskVariantsByLabVarId(labVarId);
            var labWork = new LabWorkExecutionModel(session, lab, labVarId, variants
                .Select(v => _taskExecutionModelFactory.CreateForDemoMode(
                    session,
                    v.Task.Name,
                    v.Task.Id,
                    v.Id,
                    lab.Id,
                    nextTaskLink))
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
                var tasks = GetTasksId(model.Tasks);
                var session = GetSessionGuid();
                _resultsManager.EndLabExecution(model.LabVarId, model.SessionGuid);
                ViewBag.Message = "Лабораторная работа выполнена!";
                return View("LabWorkExecutionError");
            }
            model.SetNotSolvedTaskToCurrent();

            Session[LAB_VARIABLE_KEY] = model;
            return View("Index", model);
        }

        private long[] GetTasksId(TaskExecutionModel[] tasksModel)
        {
            var result = new long[tasksModel.Length];
            for (int i = 0; i < tasksModel.Length; i++)
            {
                result[i] = tasksModel[i].TaskId;
            }
            return result;
        }
    }
}
