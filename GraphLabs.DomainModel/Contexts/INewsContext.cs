namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> Новостей GraphLabs </summary>
    public interface INewsContext
    {
        /// <summary> Новости </summary>
        IEntitySet<News> News { get; }
    }
}