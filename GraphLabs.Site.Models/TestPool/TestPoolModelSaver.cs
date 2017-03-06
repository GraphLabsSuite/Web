using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.Site.Core.OperationContext;

namespace GraphLabs.Site.Models.TestPool
{
    class TestPoolModelSaver : AbstractModelSaver<TestPoolModel, DomainModel.TestPool>
    {
        public TestPoolModelSaver(IOperationContextFactory<IGraphLabsContext> operationContextFactory) : base(operationContextFactory)
        {
        }

        protected override Action<DomainModel.TestPool> GetEntityInitializer(TestPoolModel model, IEntityQuery query)
        {
            return g =>
            {
                g.Id = model.Id;
                g.Name = model.Name;
                g.LabVariants = model.LabVariants;
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
