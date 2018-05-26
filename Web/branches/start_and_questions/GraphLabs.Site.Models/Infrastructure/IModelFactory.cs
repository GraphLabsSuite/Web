namespace GraphLabs.Site.Models.Infrastructure
{
    /// <summary> Фабрика списочных моделей </summary>
    public interface IListModelLoader
    {
        /// <summary> Создать модель списка </summary>
        TListModel LoadListModel<TListModel, TModel>()
            where TListModel : IListModel<TModel>;
    }
}