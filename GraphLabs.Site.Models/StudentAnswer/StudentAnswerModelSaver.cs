using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.TaskResults;
using GraphLabs.Site.Models.TestPool;

namespace GraphLabs.Site.Models.StudentAnswer
{
    class StudentAnswerModelSaver : AbstractModelSaver<StudentAnswerModel, DomainModel.StudentAnswer>
    {
        private readonly IOperationContextFactory<IGraphLabsContext> _operationContextFactory;

        public StudentAnswerModelSaver(
    IOperationContextFactory<IGraphLabsContext> operationContextFactory
    ) : base(operationContextFactory)
        {
            _operationContextFactory = operationContextFactory;
        }

        protected override Action<DomainModel.StudentAnswer> GetEntityInitializer(StudentAnswerModel model, IEntityQuery query)
        {
            var result = query.Get<TestResult>(model.TestResultId);
            var answer = query.Get<AnswerVariant>(model.ChosenAnswerId);
            var operation = _operationContextFactory.Create();
            var testResult = operation.DataContext.Query.OfEntities<TestResult>().FirstOrDefault(e => e.Id == result.Id);
            if (testResult != null)
            {
                testResult.Status = ExecutionStatus.Complete;
            }
            operation.Complete();
            return g =>
            {
                g.TestResult = result;
                g.AnswerVariant = answer;
                g.Time = DateTime.Now;
            };
        }

        /// <summary> Существует ли соответствующая запись в БД? </summary>
        /// <remarks> При реализации - просто проверить ключ, в базу лазить НЕ НАДО </remarks>
        protected override bool ExistsInDatabase(StudentAnswerModel model)
        {
            return model.Id != 0;
        }

        /// <summary> При реализации возвращает массив первичных ключей сущности </summary>
        protected override object[] GetEntityKey(StudentAnswerModel model)
        {
            return new object[] { model.TestResultId };
        }
    }
}
