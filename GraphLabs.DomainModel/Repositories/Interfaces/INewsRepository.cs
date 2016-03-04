using System;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel.EF.Repositories
{
    /// <summary> Репозиторий с новостями </summary>
    [ContractClass(typeof(NewsRepositoryContracts))]
    public interface INewsRepository
    {
        /// <summary> Получить новости, сортированные по дате публикации </summary>
        [NotNull]
        News[] GetNewsSortedByDate(int limit);

        /// <summary> Вытащить новость по id </summary>
        [NotNull]
        News GetById(long id);

        /// <summary> Получить новости, сортированные по дате публикации </summary>
        void Insert([NotNull]News news);
    }


    /// <summary> Репозиторий с новостями - контракты </summary>
    [ContractClassFor(typeof(INewsRepository))]
    internal abstract class NewsRepositoryContracts : INewsRepository
    {
        // ReSharper disable AssignNullToNotNullAttribute

        /// <summary> Получить новости, сортированные по дате публикации </summary>
        public News[] GetNewsSortedByDate(int limit)
        {
            Contract.Requires<ArgumentException>(limit > 0);
            Contract.Ensures(Contract.Result<News[]>() != null);

            return default(News[]);
        }

        /// <summary> Вытащить новость по id </summary>
        public News GetById(long id)
        {
            Contract.Requires<ArgumentException>(id > 0);
            Contract.Ensures(Contract.Result<News>() != null);

            return default(News);
        }

        /// <summary> Добавить в БД </summary>
        public void Insert(News news)
        {
            Contract.Requires<ArgumentNullException>(news != null);
        }
    }
}