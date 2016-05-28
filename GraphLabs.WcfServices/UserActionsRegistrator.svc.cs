using System;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.WcfServices.Data;

namespace GraphLabs.WcfServices
{
    /// <summary> Сервис предоставления данных модулям заданий </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class UserActionsRegistrator : IUserActionsRegistrator
    {
        private readonly IOperationContextFactory<IGraphLabsContext> _operationFactory;
        private readonly ISystemDateService _systemDate;

        /// <summary> Начальный балл </summary>
        private const int StartingScore = 100;

        /// <summary> Сервис предоставления данных модулям заданий </summary>
        public UserActionsRegistrator(IOperationContextFactory<IGraphLabsContext> operationFactory,
            ISystemDateService systemDate)
        {
            _operationFactory = operationFactory;
            _systemDate = systemDate;
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
            using (var op = _operationFactory.Create())
            {
                var task = op.DataContext.Query.Get<Task>(taskId);
                var session = GetSessionWithChecks(op.DataContext.Query, sessionGuid);
                var resultLog = GetCurrentResultLog(op.DataContext.Query, session);
                var taskResultLog = GetCurrentTaskResultLog(resultLog, task);

                foreach (var actionDescription in actions)
                {
                    var newAction = op.DataContext.Factory.Create<StudentAction>();
                    newAction.Description = actionDescription.Description;
                    newAction.Penalty = actionDescription.Penalty;
                    newAction.TaskResult = taskResultLog;
                    newAction.Time = actionDescription.TimeStamp;
                    taskResultLog.StudentActions.Add(newAction);
                }

                if (isTaskFinished)
                {
                    var newAction = op.DataContext.Factory.Create<StudentAction>();
                    newAction.Description = $"Задание {task.Name} выполнено.";
                    newAction.Penalty = 0;
                    newAction.TaskResult = taskResultLog;
                    newAction.Time = _systemDate.Now();
                    taskResultLog.Status = TaskResultState.Complete;
                    taskResultLog.StudentActions.Add(newAction);
                }

                var currentScore = CalculateCurrentScore(resultLog);

                op.Complete();

                return currentScore;
            }
        }

        private int CalculateCurrentScore(Result result)
        {
            return StartingScore - result.TaskResults.SelectMany(tr => tr.StudentActions).Sum(a => a.Penalty);
        }

        private Result GetCurrentResultLog(IEntityQuery query, Session session)
        {
            //TODO: Заменить Score
            var activeResults = query.OfEntities<Result>()
                .Where(result => result.Student.Id == session.User.Id && result.Score == null)
                .ToArray();

            foreach (var activeResult in activeResults.OrderByDescending(r => r.StartDateTime).Skip(1))
            {
                //TODO: Заменить Score
                activeResult.Score = -1;
            }

            return activeResults.First();
        }

        private TaskResult GetCurrentTaskResultLog(Result resultLog, Task task)
        {
            return resultLog.TaskResults.Single(tr => tr.LabEntry.Task == task);
        }

        private Session GetSessionWithChecks(IEntityQuery query, Guid sessionGuid)
        {
            var session = query.Get<Session>(sessionGuid);

            //TODO +проверка контрольной суммы и тп - всё надо куда-то в Security вытащить
            if (session.IP != HttpContext.Current.Request.UserHostAddress)
            {
                throw new EntityNotFoundException(typeof(Session), new object[] { sessionGuid });
            }

            return session;
        }
    }
}