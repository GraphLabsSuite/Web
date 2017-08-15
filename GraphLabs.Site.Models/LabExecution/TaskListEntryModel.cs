using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.LabExecution
{
    public class BaseListEntryModel
    {
        /// <summary> Выполнено? </summary>
        public TaskExecutionState State { get; set; }
    }
    
    /// <summary> Модель задания в лабе </summary>
    public class TaskListEntryModel : BaseListEntryModel
    {
        /// <summary> Id задания </summary>
        public long TaskId { get; set; }

        /// <summary> Название задания </summary>
        public string TaskName { get; set; }
    }

    public class TestListEntryModel : BaseListEntryModel
    {
        /// <summary> Id задания </summary>
        public long QuestionId { get; set; }

        /// <summary> Название тестпула </summary>
        public string TestName { get; set; }

    }

    public enum TaskExecutionState
    {
        New,
        CurrentlySolving,
        Solved
    }
}