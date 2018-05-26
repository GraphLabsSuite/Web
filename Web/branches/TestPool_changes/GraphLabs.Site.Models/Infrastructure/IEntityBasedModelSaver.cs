using GraphLabs.DomainModel.Infrastructure;

namespace GraphLabs.Site.Models.Infrastructure
{
    /// <summary> Сервис сохранения моделей обратно в базу </summary>
    /// <typeparam name="TModel">Класс модели</typeparam>
    /// <typeparam name="TEntity">Класс сущности</typeparam>
    public interface IEntityBasedModelSaver<in TModel, out TEntity>
        where TModel : IEntityBasedModel<TEntity>
        where TEntity : AbstractEntity

    {
        /// <summary> Создать или обновить сущность БД </summary>
        TEntity CreateOrUpdate(TModel model);
    }
}