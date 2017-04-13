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
        public TestPoolModelRemover(IOperationContextFactory<IGraphLabsContext> operationContextFactory)
            : base(operationContextFactory)
        {
        }
    }
}
