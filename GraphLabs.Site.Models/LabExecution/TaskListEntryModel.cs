using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.LabExecution
{
    /// <summary> Модель задания в лабе </summary>
    public class TaskListEntryModel
    {
        /// <summary> Id задания </summary>
        public long TaskId { get; set; }

        /// <summary> Название задания </summary>
        public string TaskName { get; set; }

        /// <summary> Выполнено? </summary>
        public TaskExecutionState State { get; set; }
    }

    public enum TaskExecutionState
    {
        New,
        CurrentlySolving,
        Solved
    }
}