using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.DomainModel.Infrastructure;
using GraphLabs.Site.Core.OperationContext;

namespace GraphLabs.Site.Models.Infrastructure
{
    internal class EntityRemover<TEntity> : IEntityRemover<TEntity>
        where TEntity : AbstractEntity
    {
        private readonly IOperationContextFactory<IGraphLabsContext> _operationContextFactory;

        public EntityRemover(IOperationContextFactory<IGraphLabsContext> operationContextFactory)
        {
            _operationContextFactory = operationContextFactory;
        }

        public RemovalStatus Remove(object id)
        {
            using (var operation = _operationContextFactory.Create())
            {
                var entityToDelete = operation.DataContext.Query.Find<TEntity>(id);
                if (entityToDelete == null) return RemovalStatus.ElementDoesNotExist;
                Contract.Assert(typeof (TEntity).IsAssignableFrom(typeof (TEntity)));
                try
                {
                    operation.DataContext.Factory.Delete(entityToDelete);
                    operation.Complete();
                    return RemovalStatus.Success;
                }
                catch (GraphLabsDbUpdateException e)
                {
                    if (e.ExceptionFailure == DbUpgradeFailure.FkViolated)
                    {
                        return RemovalStatus.SomeFKExistOnTheElement;
                    }
                    return RemovalStatus.UnknownFailure;
                }
            }
        }
    }
}
