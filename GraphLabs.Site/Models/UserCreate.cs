using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GraphLabs.DomainModel;
using System.Web.Mvc;
using GraphLabs.DomainModel.Extensions;

namespace GraphLabs.Site.Models
{
    public class UserCreate
    {
        public long Id { get; set; }

        [Required]
        [Display(Name="Фамилия")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Отчество")]
        public string FatherName { get; set; }

        [Display(Name = "Должность")]
        public UserRole Role { get; set; }

        [Display(Name = "Группа")]
        public long GroupID { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Первоначальный пароль")]
        [MinLength(6, ErrorMessage="Минимальная длина 6 символов")]
        public string Pass { get; set; }

		public List<SelectListItem> RoleList {
			get
			{
				var result = new List<SelectListItem>();
				result.Add(new SelectListItem { Text = "Администратор", Value = UserRole.Administrator.ValueToString() });
				result.Add(new SelectListItem { Text = "Преподаватель", Value = UserRole.Teacher.ValueToString() });
				result.Add(new SelectListItem { Text = "Студент", Value = UserRole.Student.ValueToString() });
				return result;
			}
		}
    }
}