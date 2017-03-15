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
    internal sealed class TestPoolEntryModelRemover : AbstractModelRemover<TestPoolEntryModel, DomainModel.TestPoolEntry>
    {

        private readonly IEntityBasedModelSaver<TestPoolModel, DomainModel.TestPool> _testPoolModelSaver;

        public TestPoolEntryModelRemover(
            IOperationContextFactory<IGraphLabsContext> operationContextFactory,
            IEntityBasedModelSaver<TestPoolModel, DomainModel.TestPool> testPoolModelSaver
            ) : base(operationContextFactory)
        {
            _testPoolModelSaver = testPoolModelSaver;
        }

        /// <summary> Существует ли соответствующая запись в БД? </summary>
        /// <remarks> При реализации - просто проверить ключ, в базу лазить НЕ НАДО </remarks>
        protected override bool ExistsInDatabase(long id)
        {
            return id != 0;
        }
    }
}

