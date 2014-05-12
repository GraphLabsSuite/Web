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
            return Context.News.Where(n => n.Id == id).Include(n => n.User).Single();
        }

        /// <summary> Получить новости, сортированные по дате публикации </summary>
        public void Insert(News news)
        {
            Context.News.Add(news);
        }
    }
}
