using System;

namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> Лабораторные </summary>
    [Obsolete("Использовать глобальный контекст IGraphLabsContext")]
    public interface ILabWorksContext
    {
        /// <summary> Лабораторные работы </summary>
        IEntitySet<LabWork> LabWorks { get; }
        
        /// <summary> Варианты лабораторных </summary>
        IEntitySet<LabVariant> LabVariants { get; }

        /// <summary> Задание, входящее в состав работы </summary>
        IEntitySet<LabEntry> LabEntries { get; }
    }
}