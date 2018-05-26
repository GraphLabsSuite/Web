using System.Collections.Generic;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.LabExecution
{
    /// <summary> Модель для выполнения варианта лабы </summary>
    public abstract class VariantExecutionModelBase : IEntityBasedModel<DomainModel.LabVariant>
    {
        /// <summary> ID варианта </summary>
        public long VariantId { get; set; }

        /// <summary> Название работы </summary>
        public string LabName { get; set; }

        /// <summary> Задания </summary>
        public BaseListEntryModel[] OtherTasks { get; set; }
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

    public class TestExecutionModel : VariantExecutionModelBase
    {
        /// <summary>
        /// Название тестпула
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ID вопроса
        /// </summary>
        public long QuestionId { get; set; }

        /// <summary>
        /// Текст вопроса
        /// </summary>
        public string Question { get; set; }

        public long TestResult { get; set; }

        /// <summary>
        /// Ответы на вопросы
        /// </summary>
        public ICollection<AnswerVariant> Answers { get; set; }
    }


    /// <summary> Модель для выполнения варианта лабы </summary>
    public class VariantExecutionCompleteModel : VariantExecutionModelBase
    {
        /// <summary> Название задания </summary>
        public string ResultMessage { get; set; }
    }
}