using System;
using System.Collections.Generic;
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
    abstract class AbstractModelRemover<TModel, TEntity> : IEntityBasedModelRemover<TModel, TEntity>
        where TModel : IEntityBasedModel<TEntity>
        where TEntity : AbstractEntity
    {
        private readonly IOperationContextFactory<IGraphLabsContext> _operationContextFactory;

        protected AbstractModelRemover(IOperationContextFactory<IGraphLabsContext> operationContextFactory)
        {
            _operationContextFactory = operationContextFactory;
        }

        protected virtual Type GetEntityType(TModel model)
        {
            return typeof(TEntity);
        }

        public DeletionStatus Remove(TModel model)
        {
            using (var operation = _operationContextFactory.Create())
            {
                var flag = false;
                if (ExistsInDatabase(model))
                {
                    var type = GetEntityType(model);
                    Contract.Assert(typeof(TEntity).IsAssignableFrom(type));
                    try
                    {
                        var m = operation.DataContext.Query.Get<TEntity>(GetEntityKey(model));  
                        operation.DataContext.Factory.Delete(m);
                        flag = true;
                    }
                    catch (Exception e)
                    {
                    }
                }

                operation.Complete();

                return flag ? DeletionStatus.Success : DeletionStatus.SomeFKExistOnTheElement;
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
