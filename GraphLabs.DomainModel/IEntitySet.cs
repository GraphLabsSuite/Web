using System;
using System.Collections.Generic;
using System.Linq;
using GraphLabs.DomainModel.Infrastructure;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel
{
    /// <summary> Множество сущностей определённого типа </summary>
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
}
