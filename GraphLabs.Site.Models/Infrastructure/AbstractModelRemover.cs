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

        public RemovalStatus Remove(TModel model)
        {
            using (var operation = _operationContextFactory.Create())
            {
                var isSuccessfullyRemoved = false;
                if (ExistsInDatabase(model))
                {
                    Contract.Assert(typeof(TEntity).IsAssignableFrom(typeof(TEntity)));
                    try
                    {
                        operation.DataContext.Factory.Delete(operation.DataContext.Query.Get<TEntity>(GetEntityKey(model)));
                        isSuccessfullyRemoved = true;
                    }
                    catch (SqlException e)
                    {
                    }
                }

                operation.Complete();

                return isSuccessfullyRemoved ? RemovalStatus.Success : RemovalStatus.SomeFKExistOnTheElement;
            }
        }

        /// <summary> Реализация должна возвращать элемент сущности </summary>
        protected abstract Action<TEntity> GetEntityInitializer(TModel model, IEntityQuery query);

        /// <summary> Существует ли соответствующая запись в БД? </summary>
        /// <remarks> При реализации - просто проверить ключ, в базу лазить НЕ НАДО </remarks>
        protected abstract bool ExistsInDatabase(TModel model);

        /// <summary> При реализации возвращает массив первичных ключей сущности </summary>
        protected abstract object[] GetEntityKey(TModel model);

    }
}
