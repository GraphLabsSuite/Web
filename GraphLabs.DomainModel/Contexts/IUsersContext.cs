using System.Data.Entity;

namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> Пользователи и группы </summary>
    public interface IUsersContext
    {
        /// <summary> Пользователи </summary>
        DbSet<User> Users { get; }

        /// <summary> Группы </summary>
        DbSet<Group> Groups { get; }
    }
}