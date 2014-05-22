using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.WcfServices.Data;

namespace GraphLabs.WcfServices
{
    /// <summary> Сервис предоставления данных модулям заданий </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class TasksDataService : ITasksDataService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IResultsRepository _resultsRepository;

        /// <summary> Сервис предоставления данных модулям заданий </summary>
        public TasksDataService(
            ITaskRepository taskRepository,
            ISessionRepository sessionRepository,
            IResultsRepository resultsRepository)
        {
            _taskRepository = taskRepository;
            _sessionRepository = sessionRepository;
            _resultsRepository = resultsRepository;
        }

        /// <summary> Регистрирует начало выполнения задания </summary>
        /// <param name="taskId"> Идентификатор модуля-задания </param>
        /// <param name="sessionGuid"> Идентификатор сессии </param>
        /// <returns> Данные для задания - как правило, исходный граф, или что-то типа того </returns>
        public TaskVariantInfo GetVariant(long taskId, Guid sessionGuid)
        {
            var task = GetTask(taskId);
            var session = GetSession(sessionGuid);
            var result = GetCurrentResult(session);

            var variant = result.LabVariant;
            var taskVariant = GetTaskVariant(variant, task);

            return new TaskVariantInfo
            {
                Data = taskVariant.Data,
                GeneratorVersion = taskVariant.GeneratorVersion,
                Id = taskVariant.Id,
                Number = taskVariant.Number,
                Version = taskVariant.Version
            };
        }

        private static TaskVariant GetTaskVariant(LabVariant variant, Task task)
        {
            var candidates = variant.TaskVariants.Where(v => v.Task == task).ToArray();

            if (candidates.Count() != 1)
            {
                throw new Exception(string.Format("Не удалось найти вариант для задания {0}", task.Name));
            }

            return candidates.Single();
        }

        private Result GetCurrentResult(Session session)
        {
            var activeResults = _resultsRepository.FindNotFinishedResults((Student)session.User);

            if (!activeResults.Any())
            {
                throw new Exception(string.Format("Выполнение лабораторной работы не было начато текущим пользователем."));
            }

            if (activeResults.Count() > 1)
            {
                throw new Exception(string.Format("Данным пользователем выполняется более 1 лабораторной работы. Провалищще."));
            }

            return activeResults.Single();
        }

        private Session GetSession(Guid sessionGuid)
        {
            var session = _sessionRepository.FindByGuid(sessionGuid);
            //TODO +проверка контрольной суммы и тп - всё надо куда-то в Security вытащить
            if (session == null ||
                session.IP != HttpContext.Current.Request.UserHostAddress)
            {
                throw new Exception(string.Format("Сессия с guid={0} не найдена.", sessionGuid));
            }

            return session;
        }

        private Task GetTask(long taskId)
        {
            Contract.Ensures(Contract.Result<Task>() != null);

            var task = _taskRepository.FindById(taskId);
            if (task == null)
            {
                throw new Exception(string.Format("Задание с id={0} не найдено.", taskId));
            }

            return task;
        }
    }
}
