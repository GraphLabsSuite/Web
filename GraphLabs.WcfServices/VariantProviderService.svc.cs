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
    public class VariantProviderService : IVariantProviderService
    {
        private readonly IOperationContextFactory<IGraphLabsContext> _operationFactory;
        private readonly ISystemDateService _systemDate;

        /// <summary> Сервис предоставления данных модулям заданий </summary>
        public VariantProviderService(
            IOperationContextFactory<IGraphLabsContext> operationFactory,
            ISystemDateService systemDate)
        {
            _operationFactory = operationFactory;
            _systemDate = systemDate;
        }

        /// <summary> Регистрирует начало выполнения задания </summary>
        /// <param name="taskId"> Идентификатор модуля-задания </param>
        /// <param name="sessionGuid"> Идентификатор сессии </param>
        /// <returns> Данные для задания - как правило, исходный граф, или что-то типа того </returns>
        public TaskVariantDto GetVariant(long taskId, Guid sessionGuid)
        {
            using (var op = _operationFactory.Create())
            {
                var session = GetSessionWithChecks(op.DataContext.Query, sessionGuid);

                var task = op.DataContext.Query.Get<Task>(taskId);
                var resultLog = GetCurrentResultLog(op.DataContext.Query, session);
                var taskResultLog = GetCurrentTaskResultLog(resultLog, task);

                var variant = resultLog.LabVariant;
                var taskVariant = variant.TaskVariants.Single(v => v.Task == task);

                var action = op.DataContext.Factory.Create<StudentAction>();
                action.TaskResult = taskResultLog;
                action.Time = _systemDate.Now();
                action.Description = $"['Task {task.Id}' -> Variant {taskVariant.Number}]";
                action.Penalty = 0;
                taskResultLog.StudentActions.Add(action);

                op.Complete();

                return new TaskVariantDto
                {
                    Data = taskVariant.Data,
                    GeneratorVersion = taskVariant.GeneratorVersion,
                    Id = taskVariant.Id,
                    Number = taskVariant.Number,
                    Version = taskVariant.Version
                };
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
            return resultLog.AbstractResultEntries.OfType<TaskResult>().Single(tr => tr.TaskVariant.Task == task);
        }

        private Session GetSessionWithChecks(IEntityQuery query, Guid sessionGuid)
        {
            var session = query.Get<Session>(sessionGuid);

            //TODO +проверка контрольной суммы и тп - всё надо куда-то в Security вытащить
            if (session.IP != HttpContext.Current.Request.UserHostAddress)
            {
                throw new EntityNotFoundException(typeof(Session), new object[] {sessionGuid});
            }

            return session;
        }
    }
}
