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
using GraphLabs.Site.Models.TestPool;

namespace GraphLabs.Site.Models.TestPoolEntry
{
    class TestPoolEntryModelRemover : AbstractModelRemover<TestPoolEntryModel, DomainModel.TestPoolEntry>
    {

        private readonly IEntityBasedModelSaver<TestPoolModel, DomainModel.TestPool> _testPoolModelSaver;

        public TestPoolEntryModelRemover(
            IOperationContextFactory<IGraphLabsContext> operationContextFactory,
            IEntityBasedModelSaver<TestPoolModel, DomainModel.TestPool> testPoolModelSaver
            ) : base(operationContextFactory)
        {
            _testPoolModelSaver = testPoolModelSaver;
        }

        protected override Action<DomainModel.TestPoolEntry> GetEntityInitializer(TestPoolEntryModel model, IEntityQuery query)
        {
            //var testPool = _testPoolModelSaver.CreateOrUpdate(model.TestPool);
            var m = query.Get<DomainModel.TestPoolEntry>(model.Id);
            return g =>
            {
                g.Id = model.Id;
                g.Score = model.Score;
                g.ScoringStrategy = model.ScoringStrategy;
                g.TestQuestion = model.TestQuestion;
                g.TestPool = m.TestPool;
                g.TestResults = model.TestResults;
            };
        }

        /// <summary> Существует ли соответствующая запись в БД? </summary>
        /// <remarks> При реализации - просто проверить ключ, в базу лазить НЕ НАДО </remarks>
        protected override bool ExistsInDatabase(TestPoolEntryModel model)
        {
            return model.Id != 0;
        }

        /// <summary> При реализации возвращает массив первичных ключей сущности </summary>
        protected override object[] GetEntityKey(TestPoolEntryModel model)
        {
            return new object[] { model.Id };
        }
    }
}

