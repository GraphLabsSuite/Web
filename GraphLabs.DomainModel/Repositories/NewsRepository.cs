using System.Data.Entity;
using System.Linq;

namespace GraphLabs.DomainModel.Repositories
{
    /// <summary> Репозиторий с новостями </summary>
    internal class NewsRepository : RepositoryBase, INewsRepository
    {
        /// <summary> Репозиторий с новостями </summary>
        public NewsRepository(GraphLabsContext context) : base(context)
        {
        }

        /// <summary> Получить новости, сортированные по дате публикации </summary>
        public News[] GetNewsSortedByDate(int limit)
        {
            return Context.News
                .OrderByDescending(n => n.PublicationTime)
                .Include(n => n.User)
                .Take(limit)
                .ToArray();
        }

        /// <summary> Вытащить новость по id </summary>
        public News GetById(long id)
        {
            return Context.News.SingleOrDefault(n => n.Id == id);
        }

        /// <summary> Получить новости, сортированные по дате публикации </summary>
        public News Create(string title, string text, User author)
        {
            var news = new News
            {
                Title = title,
                Text = text,
                User = author
            };

            Context.News.Add(news);

            return news;
        }
    }
}
