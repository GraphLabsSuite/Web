using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.ServiceModel.Activation;
using System.Web;
using GraphLabs.DomainModel;
using GraphLabs.Dal.Ef;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.WcfServices.Data;

namespace GraphLabs.WcfServices
{
    /// <summary> Сервис предоставления данных модулям заданий </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class UserActionsRegistrator : IUserActionsRegistrator
    {
        private readonly IReportsContext _reportsCtx;
        private readonly ITasksContext _tasksCtx;
        private readonly ISessionsContext _sessionsCtx;
        private readonly IChangesTracker _changesTracker;

        /// <summary> Начальный балл </summary>
        public const int StartingScore = 100;

        /// <summary> Сервис предоставления данных модулям заданий </summary>
        public UserActionsRegistrator(
            IReportsContext reportsCtx,
            ITasksContext tasksCtx,
            ISessionsContext sessionsCtx,
            IChangesTracker changesTracker)
        {
            _reportsCtx = reportsCtx;
            _tasksCtx = tasksCtx;
            _sessionsCtx = sessionsCtx;
            _changesTracker = changesTracker;
//            Context = new GraphLabsContext();
        }

        /// <summary> Регистрирует действия студента </summary>
        /// <param name="taskId"> Идентификатор модуля-задания </param>
        /// <param name="sessionGuid"> Идентификатор сессии </param>
        /// <param name="actions"> Действия для регистрации </param>
        /// <param name="isTaskFinished"> Задание завершено? </param>
        /// <returns> Количество баллов студента </returns>
        /// <remarks> От этой штуки зависит GraphLabs.Components </remarks>
        public int RegisterUserActions(long taskId, Guid sessionGuid, ActionDescription[] actions, bool isTaskFinished = false)
        {
            var task = GetTask(taskId);
            var session = GetSession(sessionGuid);
            var result = GetCurrentResult(session);

            foreach (var actionDescription in actions)
            {
                var newAction = _reportsCtx.Actions.CreateNew();
                newAction.Description = actionDescription.Description;
                newAction.Penalty = actionDescription.Penalty;
                newAction.Result = result;
                newAction.Time = actionDescription.TimeStamp;
                newAction.Task = task;
            }
            _changesTracker.SaveChanges();
            return CalculateCurrentScore(result);
        }

        private int CalculateCurrentScore(Result result)
        {
            return StartingScore - result.Actions.Sum(a => a.Penalty);
        }

        private Result GetCurrentResult(Session session)
        {
            var activeResults = _reportsCtx.Results
                .Query
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
            var session = _sessionsCtx.Sessions.Find(sessionGuid);
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

            var task = _tasksCtx.Tasks.Find(taskId);
            if (task == null)
            {
                throw new Exception(string.Format("Задание с id={0} не найдено.", taskId));
            }

            return task;
        }
    }
}