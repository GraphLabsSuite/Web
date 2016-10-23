using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Schedule.Edit
{
    /// <summary> Модель редактирования строки расписания </summary>
    public abstract class EditLabScheduleModelBase : IEntityBasedModel<AbstractLabSchedule>
    {
        public enum Kind
        {
            Group,
            Individual
        }

        public long Id { get; set; }

        [Required(ErrorMessage = "Необходимо указать дату открытия")]
        [Display(Name = "Дата открытия")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DateFrom { get; set; }

        [Required(ErrorMessage = "Необходимо указать дату закрытия")]
        [Display(Name = "Дата закрытия")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DateTill { get; set; }

        [Required(ErrorMessage = "Необходимо указать дату режим выполнения")]
        [Display(Name = "Режим выполнения")]
        public LabExecutionMode Mode { get; set; }

        public IDictionary<long, string> Doers { get; set; }
        [Display(Name = "Выполняет")]
        public virtual string SelectedDoerId { get; set; }

        public IDictionary<long, string> LabWorks { get; set; }
        [Display(Name = "Лабораторная работа")]
        public string SelectedLabWorkId { get; set; }

        public abstract Kind ScheduleKind { get; }

        public long GetDoerId()
        {
            return long.Parse(SelectedDoerId);
        }

        public long GetLabWorkId()
        {
            return long.Parse(SelectedLabWorkId);
        }
    }
}