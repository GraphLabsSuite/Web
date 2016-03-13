using System;

namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> Задания </summary>
    [Obsolete("Использовать глобальный контекст IGraphLabsContext")]
    public interface ITasksContext
    {
        /// <summary> Задания </summary>
        IEntitySet<Task> Tasks { get; }

        /// <summary> Варианты заданий </summary>
        IEntitySet<TaskVariant> TaskVariants { get; }
    }
}