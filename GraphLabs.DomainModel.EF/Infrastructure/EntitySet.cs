using System;
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

        /// <summary> Ищет сущность по ключу </summary>
        public TEntity Find(params object[] keyValues)
        {
            return _ctx.Set<TEntity>().Find(keyValues);
        }

        /// <summary> Ищет сущность по ключу </summary>
        /// <exception cref="EntityNotFoundException">Не удалось найти сущность с заданным ключом</exception>
        public TEntity Get(params object[] keyValues)
        {
            var entity = _ctx.Set<TEntity>().Find(keyValues);
            if (entity == null)
                throw new EntityNotFoundException(typeof(TEntity), keyValues);

            return entity;
        }

        /// <summary> Создаёт новый экземпляр сущности </summary>
        public TDerivedEntity CreateNew<TDerivedEntity>() where TDerivedEntity : TEntity
        {
            var set = _ctx.Set<TEntity>();
            var entity = set.Create<TDerivedEntity>();
            set.Add(entity);

            return entity;
        }

        /// <summary> Создаёт новый экземпляр сущности </summary>
        public TEntity CreateNew()
        {
            var set = _ctx.Set<TEntity>();
            var entity = set.Create();
            set.Add(entity);

            return entity;
        }
    }
}
