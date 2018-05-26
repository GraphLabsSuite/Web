using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.DomainModel.Infrastructure;

namespace GraphLabs.Site.Models.Infrastructure
{
    /// <summary> Базовый загрузчик модели по сущности </summary>
    /// <typeparam name="TModel">Класс модели</typeparam>
    /// <typeparam name="TEntity">Класс сущности</typeparam>
    abstract class AbstractModelLoader<TModel, TEntity> : IEntityBasedModelLoader<TModel, TEntity>
        where TModel : IEntityBasedModel<TEntity>
        where TEntity : AbstractEntity 
    {
        protected readonly IEntityQuery _query;

        /// <summary> Базовый загрузчик модели по сущности </summary>
        protected AbstractModelLoader(IEntityQuery query)
        {
            _query = query;
        }

        public TModel Load(object key)
        {
            var entity = _query.Get<TEntity>(key);
            return Load(entity);
        }

        public abstract TModel Load(TEntity entity);
    }
}