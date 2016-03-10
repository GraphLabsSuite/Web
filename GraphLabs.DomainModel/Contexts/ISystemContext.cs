namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> Системные настройки </summary>
    public interface ISystemContext
    {
        /// <summary> Системные настройки </summary>
        IEntitySet<Settings> Settings { get; }
    }
}