using System;
using System.Collections.Generic;
using System.Linq;
using GraphLabs.DomainModel.Infrastructure;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel.EF
{
    /// <summary> Множество сущностей определённого типа - реализация EF </summary>
    class EntitySet<TEntity> : IEntitySet<TEntity>
        where TEntity : AbstractEntity
    {
        private readonly GraphLabsContext _ctx;

        /// <summary> Множество сущностей определённого типа - реализация EF </summary>
        public EntitySet(GraphLabsContext ctx)
        {
            _ctx = ctx;
        }

        /// <summary> Поиск необходимых сущностей </summary>
        [NotNull]
        public IQueryable<TEntity> Query => _ctx.Set<TEntity>();

        /// <summary> Ищет сущность по ключу </summary>
        public TEntity Find(params object[] keyValues)
        {
            return _ctx.Set<TEntity>().Find(keyValues);
        }

        /// <summary> Добавление сущности </summary>
        public void Add([NotNull]TEntity entity)
        {
            _ctx.Set<TEntity>().Add(entity);
        }

        /// <summary> Массовое добавление сущностей </summary>
        public void AddRange([NotNull]IEnumerable<TEntity> entities)
        {
            _ctx.Set<TEntity>().AddRange(entities);
        }
    }
}
