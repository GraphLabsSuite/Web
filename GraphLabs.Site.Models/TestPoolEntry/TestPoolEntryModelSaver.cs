using System;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Site.Models.TestPoolEntry;

namespace GraphLabs.Site.Models.TestPool
{
    internal sealed class TestPoolEntryModelSaver : AbstractModelSaver<TestPoolEntryModel, DomainModel.TestPoolEntry>
    {
        public TestPoolEntryModelSaver(
            IOperationContextFactory<IGraphLabsContext> operationContextFactory
            ) : base(operationContextFactory)
        {
        }

        protected override Action<DomainModel.TestPoolEntry> GetEntityInitializer(TestPoolEntryModel model, IEntityQuery query)
        {
            var entity = query.Get<DomainModel.TestPoolEntry>(model.Id);
            var subCategory = query.Get<SubCategory>(model.SubCategory.Id);

            return g =>
            {
                g.Id = model.Id;
                g.SubCategory = subCategory;
                g.TestPool = entity.TestPool;
                g.QuestionsCount = model.QuestionsCount;

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
