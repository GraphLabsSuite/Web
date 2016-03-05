using System;
using System.Collections.Generic;
using System.Linq;
using GraphLabs.DomainModel.Infrastructure;

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
        public IQueryable<TEntity> Query => _ctx.Set<TEntity>();

        /// <summary> Добавление сущности </summary>
        public void Add(TEntity entity)
        {
            _ctx.Set<TEntity>().Add(entity);
        }

        /// <summary> Массовое добавление сущностей </summary>
        public void AddRange(IEnumerable<TEntity> entities)
        {
            _ctx.Set<TEntity>().AddRange(entities);
        }
    }
}
