using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.DomainModel.Services;

namespace GraphLabs.Site.Models
{
    /// <summary> Моделька для отображения во View </summary>
    public class UserModel
    {
        /// <summary> Id </summary>
        public long Id { get; set; }

        /// <summary> ФИО </summary>
        public string Name { get; set; }

        /// <summary> Email </summary>
        public string Email { get; set; }

        /// <summary> Является студентом? </summary>
        public bool IsStudent { get; set; }

        /// <summary> Группа </summary>
        public string Group { get; set; }

        /// <summary> Утверждён? </summary>
        public bool? IsVerified { get; set; }

        /// <summary> Отчислен? </summary>
        public bool? IsDismissed { get; set; }

        public UserModel()
        {
        }

        /// <summary> Конструктор для простого создания из модели </summary>
        public UserModel(User model, ISystemDateService dateService)
        {
            Id = model.Id;
            Name = string.Format("{0} {1} {2}", model.Surname, model.Name, model.FatherName);
            Email = model.Email;

            var student = model as Student;
            IsStudent = student != null;
            Group = student != null ? student.Group.GetName(dateService) : null;
            IsVerified = student != null ? student.IsVerified : (bool?)null;
            IsDismissed = student != null ? student.IsDismissed : (bool?)null;
        }
    }
}