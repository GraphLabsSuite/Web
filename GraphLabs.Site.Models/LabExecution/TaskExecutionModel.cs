using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.LabExecution
{
    /// <summary> ������ ������� � ���� </summary>
    public class TaskExecutionModel
    {
        /// <summary> Id ������� </summary>
        public long TaskId { get; set; }

        /// <summary> �������� ������� </summary>
        public string TaskName { get; set; }

        /// <summary> ���������? </summary>
        public TaskExecutionState State { get; set; }
    }

    public enum TaskExecutionState
    {
        New,
        CurrentlySolving,
        Solved
    }
}