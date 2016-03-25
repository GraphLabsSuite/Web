using System;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.News
{
    public class NewsListModel : ListModelBase<NewsModel>
    {
        private readonly IEntityQuery _query;
        private readonly IEntityBasedModelLoader<NewsModel, DomainModel.News> _modelLoader;

        /// <summary> Максимальное число отображаемых новостей </summary>
        public int MaxNewsCount { get; set; }

        public NewsListModel(
            IEntityQuery query,
            IEntityBasedModelLoader<NewsModel, DomainModel.News> modelLoader)
        {
            _query = query;
            _modelLoader = modelLoader;
        }

        /// <summary> Реализация этого метода должна загружать список моделей </summary>
        protected override NewsModel[] LoadItems()
        {
            return _query.OfEntities<DomainModel.News>()
                .OrderByDescending(n => n.LastModificationTime.HasValue ? n.LastModificationTime : n.PublicationTime)
                .Take(MaxNewsCount)
                .ToArray()
                .Select(n => _modelLoader.Load(n))
                .ToArray();
        }
    }
}