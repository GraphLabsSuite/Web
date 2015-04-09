using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GraphLabs.DomainModel.Contexts;

namespace GraphLabs.Site.Models.News
{
    /// <summary> Модель панели с новостями </summary>
    public class NewsListModel : IEnumerable<NewsModel>
    {
        private readonly INewsContext _newsContext;

        /// <summary> Количество отображаемых новостей </summary>
        public int NewsToShow { get; set; }

        /// <summary> Модель панели с новостями </summary>
        public NewsListModel(INewsContext newsContext)
        {
            _newsContext = newsContext;
        }

        private IEnumerable<NewsModel> _records;

        /// <summary> Загрузить новости </summary>
        public void Load()
        {
            _records = _newsContext.News
                .OrderByDescending(n => n.PublicationTime)
                .Include(n => n.User)
                .Take(NewsToShow)
                .ToArray()
                .Select(r => new NewsModel(r));
        }

        /// <summary> Returns an enumerator that iterates through the collection. </summary>
        public IEnumerator<NewsModel> GetEnumerator()
        {
            return _records.GetEnumerator();
        }

        /// <summary> Returns an enumerator that iterates through the collection. </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}