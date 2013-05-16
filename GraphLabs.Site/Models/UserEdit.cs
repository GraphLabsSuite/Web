using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Utils;
using System.ComponentModel.DataAnnotations;

namespace GraphLabs.Site.Models
{
    public class UserEdit
    {
        public long Id { get; set; }

        public UserRole Role { get; set; }

        [Required(ErrorMessage = "Требуется фамилия")]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Требуется имя")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Требуется отчество")]
        [Display(Name = "Отчество")]
        public string FatherName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Группа")]
        public long? GroupID { get; set; }

        public bool? IsVerified { get; set; }

        public bool? IsDismissed { get; set; }

        public UserEdit()
        {
        }

        public UserEdit(User user)
        {
            Id = user.Id;
            Surname = user.Surname;
            Name = user.Name;
            FatherName = user.FatherName;
            Email = user.Email;
            Role = user.Role;
            GroupID = null;
            IsVerified = null;
            IsDismissed = null;
        }

        public void ChangeToStudent(Student stud)
        {
            GroupID = stud.Group.Id;
            IsVerified = stud.IsVerified;
            IsDismissed = stud.IsDismissed;
        }
    }
}