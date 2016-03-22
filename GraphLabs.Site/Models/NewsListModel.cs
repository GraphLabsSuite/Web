using System;
using System.Diagnostics.Contracts;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;

namespace GraphLabs.Site.Models
{
    public class NewsListModel
    {
        private readonly IGraphLabsContext _newsContext;

        public NewsListModel(IGraphLabsContext context)
        {
            _newsContext = context;
        }

        public NewsModel[] GetNewsSortedByDate(int limit)
        {
            Contract.Requires<ArgumentException>(limit > 0);
            Contract.Ensures(Contract.Result<NewsModel[]>() != null);

            return
                _newsContext.Query<News>()
                    .OrderByDescending(n => n.LastModificationTime.HasValue ? n.LastModificationTime : n.PublicationTime)
                    .ToArray()
                    .Select(n => new NewsModel(n))
                    .ToArray();
        }
    }
}