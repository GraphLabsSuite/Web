namespace GraphLabs.Site.Models
{
    /// <summary> ������ ������� � ���� </summary>
    public class TaskExecutionModel
    {
        /// <summary> Id ������� </summary>
        public long TaskId { get; set; }

        /// <summary> �������� ������� </summary>
        public string TaskName { get; set; }

        /// <summary> ������ ������������� </summary>
        public string InitParams { get; set; }

        /// <summary> ���������? </summary>
        public bool IsSolved { get; set; }
    }
}