using System;
using System.Linq;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Infrastructure;
using GraphLabs.Site.Core.OperationContext;

namespace GraphLabs.Site.Logic
{
    public static class OperationContextExtensions
    {
        /// <summary> Запрос </summary>
        public static IQueryable<TEntity> QueryOf<TEntity>(this IOperationContext<IGraphLabsContext> operation) 
            where TEntity : AbstractEntity
        {
            return operation.DataContext.Query.OfEntities<TEntity>();
        }
    }
}
