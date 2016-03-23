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

        public NewsModel GetById(long id)
        {
            Contract.Requires<ArgumentException>(id > 0);
            Contract.Ensures(Contract.Result<NewsModel>() != null);

            return _newsContext.Query<News>().ToArray().Select(n => new NewsModel(n)).Single(n => n.Id == id);
        }

        public bool CreateOrEditNews(long id, string title, string text, string authorEmail)
        {
            Contract.Requires<ArgumentException>(id >= 0);
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(title));
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(text));
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(authorEmail));

            News news;

            var user = _newsContext.Query<User>().SingleOrDefault(u => u.Email == authorEmail &&
                                                                        (!(u is Student) ||
                                                                         ((u as Student).IsVerified &&
                                                                          !(u as Student).IsDismissed)));
            if (user == null)
            {
                return false;
            }
            if (id == 0)
            {
                news = _newsContext.Create<News>();
                news.Title = title;
                news.Text = text;
                news.User = user;

                return true;
            }
            else
            {
                news = _newsContext.Query<News>().Single(n => n.Id == id);

                if (news.User != user && !user.Role.HasFlag(UserRole.Administrator))
                {
                    return false;
                }

                news.Text = text;
                news.Title = title;
                news.User = user;

                return true;
            }
        }
    }
}