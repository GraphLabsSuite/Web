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

namespace GraphLabs.Site.Models.TestPool
{
    internal sealed class TestPoolModelRemover : AbstractModelRemover<TestPoolModel, DomainModel.TestPool>
    {
        public TestPoolModelRemover(IOperationContextFactory<IGraphLabsContext> operationContextFactory) : base(operationContextFactory)
        {
    }

        protected override Action<DomainModel.TestPool> GetEntityInitializer(TestPoolModel model, IEntityQuery query)
        {
            var entity = query.Get<DomainModel.TestPool>(model.Id);
            return g =>
            {
                g.Id = model.Id;
                g.Name = model.Name;
                g.LabVariants = entity.LabVariants;
                g.TestPoolEntries = entity.TestPoolEntries;
            };
        }

        /// <summary> Существует ли соответствующая запись в БД? </summary>
        /// <remarks> При реализации - просто проверить ключ, в базу лазить НЕ НАДО </remarks>
        protected override bool ExistsInDatabase(TestPoolModel model)
    {
        return model.Id != 0;
    }

    /// <summary> При реализации возвращает массив первичных ключей сущности </summary>
    protected override object[] GetEntityKey(TestPoolModel model)
    {
        return new object[] { model.Id };
    }
}
}
