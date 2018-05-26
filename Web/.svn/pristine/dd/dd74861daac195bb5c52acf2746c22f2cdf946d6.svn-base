using System;

namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> Журнал выполнения заданий </summary>
    [Obsolete("Использовать глобальный контекст IGraphLabsContext")]
    public interface IReportsContext
    {
        /// <summary> Результаты </summary>
        IEntitySet<Result> Results { get; }

        /// <summary> Результаты заданий </summary>
        IEntitySet<TaskResult> TaskResults { get; }

            /// <summary> Журнал действий </summary>
        IEntitySet<StudentAction> Actions { get; }
    }
}