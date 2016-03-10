namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> Пользователи и группы </summary>
    public interface IUsersContext
    {
        /// <summary> Пользователи </summary>
        IEntitySet<User> Users { get; }

        /// <summary> Группы </summary>
        IEntitySet<Group> Groups { get; }
    }
}