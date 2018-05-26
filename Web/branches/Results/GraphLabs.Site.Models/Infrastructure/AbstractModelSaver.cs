using System;
using System.Diagnostics.Contracts;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.DomainModel.Infrastructure;
using GraphLabs.Site.Core.OperationContext;

namespace GraphLabs.Site.Models.Infrastructure
{
    /// <summary> Сервис сохранения групп </summary>
    abstract class AbstractModelSaver<TModel, TEntity> : IEntityBasedModelSaver<TModel, TEntity>
        where TModel : IEntityBasedModel<TEntity>
        where TEntity : AbstractEntity
    {
        private readonly IOperationContextFactory<IGraphLabsContext> _operationContextFactory;

        protected AbstractModelSaver(IOperationContextFactory<IGraphLabsContext> operationContextFactory)
        {
            _operationContextFactory = operationContextFactory;
        }

        protected virtual Type GetEntityType(TModel model)
        {
            return typeof(TEntity);
        }

        public TEntity CreateOrUpdate(TModel model)
        {
            using (var operation = _operationContextFactory.Create())
            {
                TEntity entity;
                if (!ExistsInDatabase(model))
                {
                    var type = GetEntityType(model);
                    Contract.Assert(typeof(TEntity).IsAssignableFrom(type));
                    entity = (TEntity)operation.DataContext.Factory.Create(
                        type,
                        o => GetEntityInitializer(model, operation.DataContext.Query)((TEntity)o));
                }
                else
                {
                    entity = operation.DataContext.Query.Get<TEntity>(GetEntityKey(model));
                    GetEntityInitializer(model, operation.DataContext.Query).Invoke(entity);
                }

                operation.Complete();

                return entity;
            }
        }

        /// <summary> Реализация должна возвращать выражение-инициализатор сущности </summary>
        protected abstract Action<TEntity> GetEntityInitializer(TModel model, IEntityQuery query);

        /// <summary> Существует ли соответствующая запись в БД? </summary>
        /// <remarks> При реализации - просто проверить ключ, в базу лазить НЕ НАДО </remarks>
        protected abstract bool ExistsInDatabase(TModel model);

        /// <summary> При реализации возвращает массив первичных ключей сущности </summary>
        protected abstract object[] GetEntityKey(TModel model);
    }
}