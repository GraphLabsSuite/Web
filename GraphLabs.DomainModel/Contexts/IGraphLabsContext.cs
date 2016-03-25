using System;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> Контекст GraphLabs </summary>
    public interface IGraphLabsContext : IDisposable
    {
        /// <summary> Запрос сущностей </summary>
        [NotNull]
        IEntityQuery Query { get; }

        /// <summary> Фабрика сущностей </summary>
        [NotNull]
        IEntityFactory Factory { get; }
    }
}