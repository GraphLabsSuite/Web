using System.ComponentModel.DataAnnotations;

namespace GraphLabs.Site.Models.Schedule.Edit
{
    /// <summary> ������ �������������� ������ ���������� </summary>
    public class EditIndividualLabScheduleModel : EditLabScheduleModelBase
    {
        [Required(ErrorMessage = "���������� ������� ��������")]
        [Display(Name = "�������")]
        public override string SelectedDoerId { get; set; }

        public override Kind ScheduleKind => Kind.Individual;
    }
}