using System;
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
        [NotNull]
        IQueryable<TEntity> Query { get; }

        /// <summary> Ищет сущность по ключу </summary>
        [CanBeNull]
        TEntity Find(params object[] keyValue);

        /// <summary> Создаёт новый экземпляр сущности </summary>
        [NotNull]
        TDerivedEntity CreateNew<TDerivedEntity>() where TDerivedEntity : TEntity;

        /// <summary> Создаёт новый экземпляр сущности </summary>
        [NotNull]
        TEntity CreateNew();
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

        /// <summary> Создаёт новый экземпляр сущности </summary>
        public TDerivedEntity CreateNew<TDerivedEntity>() where TDerivedEntity : TEntity
        {
            Contract.Ensures(Contract.Result<TDerivedEntity>() != null);
            return default(TDerivedEntity);
        }

        /// <summary> Создаёт новый экземпляр сущности </summary>
        public TEntity CreateNew()
        {
            Contract.Ensures(Contract.Result<TEntity>() != null);
            return default(TEntity);
        }

        /// <summary> Ищет сущность по ключу </summary>
        public TEntity Find(params object[] keyValue)
        {
            Contract.Requires<ArgumentException>(keyValue.Length > 0);
            return default(TEntity);
        }
    }
}
