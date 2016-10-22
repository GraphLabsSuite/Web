using System.ComponentModel.DataAnnotations;

namespace GraphLabs.Site.Models.Schedule.Edit
{
    /// <summary> Модель редактирования строки расписания </summary>
    public class EditIndividualLabScheduleModel : EditLabScheduleModelBase
    {
        [Required(ErrorMessage = "Необходимо указать студента")]
        [Display(Name = "Студент")]
        public override string SelectedDoerId { get; set; }

        public override Kind ScheduleKind => Kind.Individual;
    }
}