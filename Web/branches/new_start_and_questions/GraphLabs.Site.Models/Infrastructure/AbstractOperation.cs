using System;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.Site.Core.OperationContext;

namespace GraphLabs.Site.Models.Infrastructure
{
    internal abstract class AbstractOperation : IDisposable
    {
        private readonly IOperationContext<IGraphLabsContext> _operation;

        protected IEntityQuery Query => _operation.DataContext.Query;
        protected IEntityFactory Factory => _operation.DataContext.Factory;

        protected void Complete()
        {
            _operation.Complete();
        }

        protected AbstractOperation(IOperationContextFactory<IGraphLabsContext> operationFactory)
        {
            _operation = operationFactory.Create();
        }

        public void Dispose()
        {
            _operation.Dispose();
        }
    }
}