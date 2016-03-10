using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.EF;
using GraphLabs.WcfServices.Data;

namespace GraphLabs.WcfServices
{
    /// <summary> Сервис предоставления данных модулям заданий </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class UserActionsRegistrator : IUserActionsRegistrator, IDisposable
    {
        /// <summary> Начальный балл </summary>
        public const int StartingScore = 100;

        /// <summary> Предоставляет доступ к данным </summary>
        protected GraphLabsContext Context { get; private set; }

        /// <summary> Сервис предоставления данных модулям заданий </summary>
        public UserActionsRegistrator()
        {
            Context = new GraphLabsContext();
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
                var newAction = Context.StudentActions.Create();
                newAction.Description = actionDescription.Description;
                newAction.Penalty = actionDescription.Penalty;
                newAction.Result = result;
                newAction.Time = actionDescription.TimeStamp;
                newAction.Task = task;
            }
            Context.SaveChanges();
            return CalculateCurrentScore(result);
        }

        private int CalculateCurrentScore(Result result)
        {
            return StartingScore - result.Actions.Sum(a => a.Penalty);
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

        /// <summary> Деструктор </summary>
        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
                Context = null;
            }
        }
    }
}