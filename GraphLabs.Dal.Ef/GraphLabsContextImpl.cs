using System;
using System.Diagnostics.Contracts;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Infrastructure;


namespace GraphLabs.Dal.Ef
{
    class GraphLabsContextImpl : IGraphLabsContext, IEntityQuery, IEntityFactory
    {
        private readonly GraphLabsContext _ctx;
        private readonly bool _ownsContext;

        public GraphLabsContextImpl()
        {
            _ownsContext = true;
            _ctx = new GraphLabsContext();
        }

        public GraphLabsContextImpl(GraphLabsContext ctx)
        {
            _ownsContext = false;
            _ctx = ctx;
        }

        #region IGraphLabsContext

        public IEntityQuery Query => this;
        public IEntityFactory Factory => this;

        #endregion


        private Type GetEntityTypeFor(Type type)
        {
            Guard.AreAssignedTypes(typeof(AbstractEntity), type); 

            var baseType = type.BaseType;
            if (baseType == typeof(AbstractEntity))
            {
                return type;
            }

            return GetEntityTypeFor(baseType);
        }

        /// <summary> Поиск сущности (может не найти) </summary>
        public TEntity Find<TEntity>(object[] keyValues) where TEntity : AbstractEntity
        {
            var baseEntityType = GetEntityTypeFor(typeof(TEntity));
            return baseEntityType == typeof(TEntity)
                ? _ctx.Set<TEntity>().Find(keyValues)
                : (TEntity)_ctx.Set(baseEntityType).Find(keyValues);
        }

        /// <summary> Создаёт новый экземпляр сущности </summary>
        public TEntity Create<TEntity>(Action<TEntity> initializer) where TEntity : AbstractEntity
        {
            return (TEntity) Create(typeof (TEntity), o => initializer?.Invoke((TEntity)o));
        }

        /// <summary> Создаёт новый экземпляр сущности </summary>
        public object Create(Type typeOfEntity, Action<object> initializer)
        {
            var baseEntityType = GetEntityTypeFor(typeOfEntity);
            object newEntity;
            if (baseEntityType == typeOfEntity)
            {
                newEntity = _ctx.Set(typeOfEntity).Create();
                _ctx.Set(typeOfEntity).Add(newEntity);
            }
            else
            {
                newEntity = _ctx.Set(baseEntityType).Create(typeOfEntity);
                _ctx.Set(baseEntityType).Add(newEntity);
            }

            initializer?
                .Invoke(newEntity);

            return newEntity;
        }

        /// <summary> Удалить сущность </summary>
        public void Delete<TEntity>(TEntity entity) where TEntity : AbstractEntity
        {
            var baseEntityType = GetEntityTypeFor(typeof(TEntity));
            _ctx.Set(baseEntityType).Remove(entity);
        }

        /// <summary> Запрос сущностей </summary>
        public IQueryable<TEntity> OfEntities<TEntity>() where TEntity : AbstractEntity
        {
            var baseEntityType = GetEntityTypeFor(typeof(TEntity));
            return baseEntityType == typeof(TEntity)
                ? _ctx.Set<TEntity>()
                : _ctx.Set(baseEntityType).OfType<TEntity>();
        }

        public void Dispose()
        {
            if (_ownsContext)
            {
                _ctx.Dispose();
            }
        }
    }
}
