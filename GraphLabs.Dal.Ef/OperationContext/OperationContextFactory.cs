using GraphLabs.Dal.Ef.Infrastructure;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.Site.Core;
using GraphLabs.Site.Core.OperationContext;

namespace GraphLabs.Dal.Ef.OperationContext
{
    /// <summary> Фабрика <see cref="OperationContextImpl"/> </summary>
    class OperationContextFactory : IOperationContextFactory<IGraphLabsContext>
    {
        /// <summary> Создать контекст </summary>
        public IOperationContext<IGraphLabsContext> Create()
        {
            var ctx = new GraphLabsContext();
            return new OperationContextImpl(new GraphLabsContextImpl(ctx), new ChangesTracker(ctx));
        }
    }
}