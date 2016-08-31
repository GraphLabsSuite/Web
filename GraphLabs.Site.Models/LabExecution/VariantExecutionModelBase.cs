using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.LabExecution
{
    /// <summary> Модель для выполнения варианта лабы </summary>
    public abstract class VariantExecutionModelBase : IEntityBasedModel<LabVariant>
    {
        /// <summary> ID варианта </summary>
        public long VariantId { get; set; }

        /// <summary> Название работы </summary>
        public string LabName { get; set; }

        /// <summary> Задания </summary>
        public TaskExecutionModel[] OtherTasks { get; set; }
    }

    /// <summary> Модель для выполнения варианта лабы </summary>
    public class TaskVariantExecutionModel : VariantExecutionModelBase
    {
        /// <summary> Название задания </summary>
        public string TaskName { get; set; }

        /// <summary> Id задания </summary>
        public long TaskId { get; set; }

        /// <summary> Строка инициализации </summary>
        public string InitParams { get; set; }
    }


    /// <summary> Модель для выполнения варианта лабы </summary>
    public class VariantExecutionCompleteModel : VariantExecutionModelBase
    {
        /// <summary> Название задания </summary>
        public string ResultMessage { get; set; }
    }
}