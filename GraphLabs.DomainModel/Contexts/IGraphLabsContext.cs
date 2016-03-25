using System;
using System.Diagnostics.Contracts;
using System.Linq;
using GraphLabs.DomainModel.Infrastructure;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> Контекст GraphLabs </summary>
    [ContractClass(typeof(GraphLabsContextContracts))]
    public interface IGraphLabsContext
    {
        /// <summary> Запрос сущностей </summary>
        [NotNull]
        IQueryable<TEntity> Query<TEntity>() where TEntity : AbstractEntity;

        /// <summary> Поиск сущности (может не найти) </summary>
        [CanBeNull]
        TEntity Find<TEntity>(params object[] keyValues) where TEntity : AbstractEntity;

        /// <summary> Создаёт новый экземпляр сущности </summary>
        [NotNull]
        TEntity Create<TEntity>() where TEntity : AbstractEntity;
    }

    /// <summary> Контракты для <see cref="IGraphLabsContext"/> </summary>
    [ContractClassFor(typeof(IGraphLabsContext))]
    abstract class GraphLabsContextContracts : IGraphLabsContext
    {
        /// <summary> Запрос сущностей </summary>
        public IQueryable<TEntity> Query<TEntity>() where TEntity : AbstractEntity
        {
            Contract.Ensures(Contract.Result<IQueryable<TEntity>>() != null);
            return default(IQueryable<TEntity>);
        }

        /// <summary> Запрос сущностей </summary>
        public TEntity Find<TEntity>(params object[] keyValues) where TEntity : AbstractEntity
        {
            Contract.Requires<ArgumentNullException>(keyValues != null);
            Contract.Requires<ArgumentException>(keyValues.Any());
            Contract.Requires<ArgumentException>(keyValues.All(v => v != null));

            return default(TEntity);
        }

        /// <summary> Создаёт новый экземпляр сущности </summary>
        public TEntity Create<TEntity>() where TEntity : AbstractEntity
        {
            Contract.Ensures(Contract.Result<TEntity>() != null);

            return default(TEntity);
        }
    }
}