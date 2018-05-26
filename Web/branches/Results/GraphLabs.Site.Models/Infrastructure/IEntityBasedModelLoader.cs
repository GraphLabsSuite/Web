using GraphLabs.DomainModel.Infrastructure;

namespace GraphLabs.Site.Models.Infrastructure
{
    /// <summary> Загрузчик моделей, основанных на сущностях </summary>
    /// <typeparam name="TModel">Класс модели</typeparam>
    /// <typeparam name="TEntity">Класс сущности</typeparam>
    public interface IEntityBasedModelLoader<out TModel, in TEntity> 
        where TModel: IEntityBasedModel<TEntity>
        where TEntity : AbstractEntity

    {
        /// <summary> Загрузить по сущности-прототипу </summary>
        TModel Load(TEntity entity);

        /// <summary> Загрузить по ключу сущности-прототипа </summary>
        TModel Load(object key);
    }
}