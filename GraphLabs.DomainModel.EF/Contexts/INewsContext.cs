using System.Data.Entity;

namespace GraphLabs.DomainModel.EF.Contexts
{
    /// <summary> Новостей GraphLabs </summary>
    public interface INewsContext
    {
        /// <summary> Новости </summary>
        DbSet<News> News { get; }
    }
}