using System.ComponentModel.DataAnnotations;

namespace GraphLabs.Site.Models.Schedule.Edit
{
    /// <summary> Модель редактирования строки расписания </summary>
    public class EditGroupLabScheduleModel : EditLabScheduleModelBase
    {
        [Required(ErrorMessage = "Необходимо указать группу")]
        [Display(Name = "Группа")]
        public override string SelectedDoerId { get; set; }

        public override Kind ScheduleKind => Kind.Group;
    }
}