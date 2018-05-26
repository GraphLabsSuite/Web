using System;

namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> Новостей GraphLabs </summary>
    [Obsolete("Использовать глобальный контекст IGraphLabsContext")]
    public interface INewsContext
    {
        /// <summary> Новости </summary>
        IEntitySet<News> News { get; }
    }
}