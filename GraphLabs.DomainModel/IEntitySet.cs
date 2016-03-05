using System;
using System.Collections.Generic;
using System.Linq;
using GraphLabs.DomainModel.Infrastructure;

namespace GraphLabs.DomainModel
{
    /// <summary> Множество сущностей определённого типа </summary>
    public interface IEntitySet<TEntity>
        where TEntity: AbstractEntity
    {
        /// <summary> Поиск необходимых сущностей </summary>
        IQueryable<TEntity> Query { get; }

        /// <summary> Добавление сущности </summary>
        void Add(TEntity entity);

        /// <summary> Массовое добавление сущностей </summary>
        void AddRange(IEnumerable<TEntity> entities);
    }
}
