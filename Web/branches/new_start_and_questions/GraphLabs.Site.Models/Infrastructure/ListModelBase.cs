using System;
using System.Collections;
using System.Collections.Generic;

namespace GraphLabs.Site.Models.Infrastructure
{
    /// <summary> Модель списка </summary>
    /// <typeparam name="TModel">Класс моделей, из которых состоит список</typeparam>
    public abstract class ListModelBase<TModel> : IListModel<TModel>
    {
        /// <summary> Returns an enumerator that iterates through the collection. </summary>
        public IEnumerator<TModel> GetEnumerator()
        {
            return ((IEnumerable<TModel>)Items).GetEnumerator();
        }

        /// <summary> Returns an enumerator that iterates through a collection. </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <summary> Модель списка </summary>
        protected ListModelBase()
        {
            InvalidateItems();
        }

        /// <summary>
        /// Пересоздаёт коллекцию <see cref="Items"/>
        /// (например, если меняются фильтры)
        /// </summary>
        protected void InvalidateItems()
        {
            _itemsLazy = new Lazy<TModel[]>(LoadItems);
        }

        /// <summary> Реализация этого метода должна загружать список моделей </summary>
        protected abstract TModel[] LoadItems();
        private Lazy<TModel[]> _itemsLazy;


        /// <summary> Список </summary>
        public TModel[] Items => _itemsLazy.Value;
    }
}