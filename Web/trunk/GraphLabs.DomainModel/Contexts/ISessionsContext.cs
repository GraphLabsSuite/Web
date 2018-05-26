using System;

namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> Сессии пользователей </summary>
    [Obsolete("Использовать глобальный контекст IGraphLabsContext")]
    public interface ISessionsContext
    {
        /// <summary> Пользователи </summary>
        IEntitySet<User> Users { get; }

        /// <summary> Сессии пользователей </summary>
        IEntitySet<Session> Sessions { get; }
    }
}