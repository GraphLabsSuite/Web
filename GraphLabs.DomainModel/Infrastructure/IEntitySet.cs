using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using GraphLabs.DomainModel.Infrastructure;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel
{
    /// <summary> Множество сущностей определённого типа </summary>
    [ContractClass(typeof(EntitySetContracts<>))]
    public interface IEntitySet<TEntity>
        where TEntity: AbstractEntity
    {
        /// <summary> Поиск необходимых сущностей </summary>
        IQueryable<TEntity> Query { get; }

        /// <summary> Ищет сущность по ключу </summary>
        [CanBeNull]
        TEntity Find(params object[] keyValue);

        /// <summary> Добавление сущности </summary>
        void Add(TEntity entity);

        /// <summary> Массовое добавление сущностей </summary>
        void AddRange(IEnumerable<TEntity> entities);
    }

    /// <summary> Контракты <see cref="IEntitySet{TEntity}"/> </summary>
    [ContractClassFor(typeof(IEntitySet<>))]
    abstract class EntitySetContracts<TEntity> : IEntitySet<TEntity> where TEntity : AbstractEntity
    {
        /// <summary> Поиск необходимых сущностей </summary>
        public IQueryable<TEntity> Query
        {
            get
            {
                Contract.Ensures(Contract.Result<IQueryable<TEntity>>() != null);
                return default(IQueryable<TEntity>);
            }
        }

        /// <summary> Добавление сущности </summary>
        public void Add(TEntity entity)
        {
            Contract.Requires<ArgumentNullException>(entity != null);
        }

        /// <summary> Массовое добавление сущностей </summary>
        public void AddRange(IEnumerable<TEntity> entities)
        {
            Contract.Requires<ArgumentNullException>(entities != null);
        }

        /// <summary> Ищет сущность по ключу </summary>
        public TEntity Find(params object[] keyValue)
        {
            Contract.Requires<ArgumentException>(keyValue.Length > 0);
            return default(TEntity);
        }
    }
}
