using System;

namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> Пользователи и группы </summary>
    [Obsolete("Использовать глобальный контекст IGraphLabsContext")]
    public interface IUsersContext
    {
        /// <summary> Пользователи </summary>
        IEntitySet<User> Users { get; }

        /// <summary> Группы </summary>
        IEntitySet<Group> Groups { get; }
    }
}