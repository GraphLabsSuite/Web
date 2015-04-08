using System.Data.Entity;

namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> Сессии пользователей </summary>
    public interface ISessionsContext
    {
        /// <summary> Пользователи </summary>
        DbSet<User> Users { get; }

        /// <summary> Сессии пользователей </summary>
        DbSet<Session> Sessions { get; set; }
    }
}