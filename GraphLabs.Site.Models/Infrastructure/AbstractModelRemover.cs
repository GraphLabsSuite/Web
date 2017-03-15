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
    internal abstract class AbstractModelRemover<TModel, TEntity> : IEntityBasedModelRemover<TModel, TEntity>
        where TModel : IEntityBasedModel<TEntity>
        where TEntity : AbstractEntity
    {
        private readonly IOperationContextFactory<IGraphLabsContext> _operationContextFactory;

        protected AbstractModelRemover(IOperationContextFactory<IGraphLabsContext> operationContextFactory)
        {
            _operationContextFactory = operationContextFactory;
        }

        public RemovalStatus Remove(long id)
        {
            using (var operation = _operationContextFactory.Create())
            {
                if (ExistsInDatabase(id))
                {
                    Contract.Assert(typeof(TEntity).IsAssignableFrom(typeof(TEntity)));
                    try
                    {
                        operation.DataContext.Factory.Delete(operation.DataContext.Query.Get<TEntity>(id));
                        return RemovalStatus.Success;
                    }
                    catch (SqlException e)
                    {
                        //TODO: Узнать, какой ErrorCode возвращается в случае наличия FK
                        if (e.ErrorCode == 1)
                        {
                            
                        }
                        return RemovalStatus.SomeFKExistOnTheElement;
                    }
                    finally
                    {
                        operation.Complete();
                    }
                }
                return RemovalStatus.ElementDoesNotExist;
            }
        }

        /// <summary> Существует ли соответствующая запись в БД? </summary>
        /// <remarks> При реализации - просто проверить ключ, в базу лазить НЕ НАДО </remarks>
        protected abstract bool ExistsInDatabase(long id);

    }
}
