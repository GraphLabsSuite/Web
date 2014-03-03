using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using GraphLabs.DomainModel;
using GraphLabs.WcfServices.Data;

namespace GraphLabs.WcfServices
{
    /// <summary> Сервис предоставления данных модулям заданий </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class TasksDataService : ITasksDataService
    {
        /// <summary> Предоставляет доступ к данным </summary>
        protected GraphLabsContext Context { get; private set; }

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
            var activeResults = Context.Results
                .Where(result => result.Student == session.User && result.Grade == null)
                .ToArray();

            if (!activeResults.Any())
            {
                throw new Exception(string.Format("Выполнение лабораторной работы не было начато текущим пользователем."));
            }

            if (activeResults.Count() > 1)
            {
                FinishOldResults(activeResults);
            }

            return activeResults.First();
        }

        private void FinishOldResults(IEnumerable<Result> activeResults)
        {
            foreach (var activeResult in activeResults.OrderByDescending(r => r.StartDateTime).Skip(1))
            {
                activeResult.Grade = Grade.Interrupted;
            }
        }

        private Session GetSession(Guid sessionGuid)
        {
            var session = Context.Sessions.Find(sessionGuid);
            //TODO +проверка контрольной суммы и тп
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

            var task = Context.Tasks.Find(taskId);
            if (task == null)
            {
                throw new Exception(string.Format("Задание с id={0} не найдено.", taskId));
            }

            return task;
        }
    }
}
