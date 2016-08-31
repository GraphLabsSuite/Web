using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.ServiceModel.Activation;
using System.Web;
using GraphLabs.Dal.Ef.Repositories;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Site.Logic;
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
        public int? RegisterUserActions(long taskId, Guid sessionGuid, ActionDescription[] actions, bool isTaskFinished = false)
        {
            using (var op = _operationFactory.Create())
            {
                var task = op.DataContext.Query.Get<Task>(taskId);
                var session = GetSessionWithChecks(op.DataContext.Query, sessionGuid);
                var resultLog = GetCurrentResultLog(op.DataContext.Query, session);
                var taskResultLog = GetCurrentTaskResultLog(resultLog, task);
                if (taskResultLog.Score == null)
                {
                    taskResultLog.Score = StartingScore;
                }

                if (actions.Any())
                {
                    foreach (var actionDescription in actions)
                    {
                        var newAction = op.DataContext.Factory.Create<StudentAction>();
                        newAction.Description = actionDescription.Description;
                        newAction.Penalty = actionDescription.Penalty;
                        newAction.TaskResult = taskResultLog;
                        newAction.Time = actionDescription.TimeStamp;

                        taskResultLog.StudentActions.Add(newAction);
                    }

                    taskResultLog.Score -= actions.Last().Penalty;
                }

                if (isTaskFinished)
                {
                    var newAction = op.DataContext.Factory.Create<StudentAction>();
                    newAction.Description = $"Задание {task.Name} выполнено.";
                    newAction.Penalty = 0;
                    newAction.TaskResult = taskResultLog;
                    newAction.Time = _systemDate.Now();
                    taskResultLog.Status = ExecutionStatus.Complete;
                    taskResultLog.StudentActions.Add(newAction);

          /*          if (IsLabFinished(op, taskResultLog))
                    {
                        var student = GetCurrentStudent(op, sessionGuid);
                        //Найти неоконченные результаты выполнения

                        var resultsToInterrupt =
                            op.DataContext.Query.OfEntities<Result>()
                                .Where(
                                    result =>
                                        result.Student.Id == student.Id && result.Status == ExecutionStatus.Executing)
                                .ToArray();

                        var variant = GetLabVariant(taskResultLog);
                        // Найдём результаты, относящиеся к варианту ЛР, который пытаемся начать выполнять
                        var currentResults = resultsToInterrupt
                            .Where(res => res.LabVariant == variant)
                            .OrderByDescending(res => res.StartDateTime)
                            .ToArray();

                        // Посмотрим, есть ли вообще такие. Если есть, берём самый свежий (теоретически, там больше 1 и не должно быть).
                        var latestCurrentResult = currentResults.FirstOrDefault();
                        var taskResults = latestCurrentResult.TaskResults;
                        var mark = GetMark(taskResults);
                        latestCurrentResult.Score = mark;
                        latestCurrentResult.Status = ExecutionStatus.Complete;
                    }*/
                }

                op.Complete();

                return taskResultLog.Score;
            }
        }

        private Result GetCurrentResultLog(IEntityQuery query, Session session)
        {
            return query.OfEntities<Result>()
                .Where(result => result.Student.Id == session.User.Id && result.Status == ExecutionStatus.Executing)
                .ToArray().First();
        }

        private TaskResult GetCurrentTaskResultLog(Result resultLog, Task task)
        {
            return resultLog.TaskResults.Single(tr => tr.TaskVariant.Task == task);
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