using System.Data.Entity;

namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> Новостей GraphLabs </summary>
    public interface INewsContext
    {
        /// <summary> Новости </summary>
        DbSet<News> News { get; }

        /// <summary> Пользователи </summary>
        DbSet<User> Users { get; }
    }
}