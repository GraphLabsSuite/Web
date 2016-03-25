using System.Collections.Generic;

namespace GraphLabs.Site.Models.Infrastructure
{
    /// <summary> Модель списка </summary>
    /// <typeparam name="TModel">Класс моделей, из которых состоит список</typeparam>
    public interface IListModel<out TModel> : IEnumerable<TModel>
    {
        /// <summary> Список </summary>
        TModel[] Items { get; }
    }
}