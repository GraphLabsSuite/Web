using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Infrastructure;

namespace GraphLabs.Site.Models.Infrastructure
{
    /// <summary> ������� ��������� ������ �� �������� </summary>
    /// <typeparam name="TModel">����� ������</typeparam>
    /// <typeparam name="TEntity">����� ��������</typeparam>
    abstract class AbstractModelLoader<TModel, TEntity> : IEntityBasedModelLoader<TModel, TEntity>
        where TModel : IEntityBasedModel<TEntity>
        where TEntity : AbstractEntity 
    {
        private readonly IEntityQuery _query;

        /// <summary> ������� ��������� ������ �� �������� </summary>
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