using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Schedule.Edit
{
    /// <summary> Модель редактирования строки расписания </summary>
    public class EditLabScheduleModel : IEntityBasedModel<AbstractLabSchedule>
    {
        public enum DoerKind
        {
            Student,
            Group
        }

        [Display(Name = "Дата открытия")]
        public DateTime DateFrom { get; set; }

        [Display(Name = "Дата закрытия")]
        public DateTime DateTill { get; set; }

        [Display(Name = "Режим выполнения")]
        public LabExecutionMode Mode { get; set; }

        public IDictionary<long, string> Students { get; set; }
        public IDictionary<long, string> Groups { get; set; }

        public long SelectedDoerId { get; set; }
        public DoerKind SelectedDoerKind { get; set; }

        public bool CanChangeDoerKind { get; set; } = true;
    }
}