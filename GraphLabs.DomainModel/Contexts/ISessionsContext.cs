namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> Сессии пользователей </summary>
    public interface ISessionsContext
    {
        /// <summary> Пользователи </summary>
        IEntitySet<User> Users { get; }

        /// <summary> Сессии пользователей </summary>
        IEntitySet<Session> Sessions { get; set; }
    }
}