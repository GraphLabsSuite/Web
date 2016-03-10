namespace GraphLabs.Site.Models
{
    /// <summary> Модель задания в лабе </summary>
    public class TaskExecutionModel
    {
        /// <summary> Id задания </summary>
        public long TaskId { get; set; }

        /// <summary> Название задания </summary>
        public string TaskName { get; set; }

        /// <summary> Строка инициализации </summary>
        public string InitParams { get; set; }

        /// <summary> Выполнено? </summary>
        public bool IsSolved { get; set; }
    }
}