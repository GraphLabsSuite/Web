using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DomainModel.EF;
using GraphLabs.DomainModel.EF.Extensions;
using GraphLabs.DomainModel.EF.Services;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.Site.Logic.Security;

namespace GraphLabs.Site.Models.Account
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

		public List<SelectListItem> RoleList
		{
			get
			{
				var result = new List<SelectListItem>();
				result.Add(new SelectListItem { Text = "Администратор", Value = UserRole.Administrator.ValueToString() });
				result.Add(new SelectListItem { Text = "Преподаватель", Value = UserRole.Teacher.ValueToString() });
				result.Add(new SelectListItem { Text = "Студент", Value = UserRole.Student.ValueToString() });
				return result;
			}
		}

		public List<SelectListItem> GroupList { get; private set; }
		public void FillGroupList(Group[] groups, ISystemDateService systemDateService)
		{
			GroupList = groups
				.Select(t => new SelectListItem { Text = t.GetName(systemDateService), Value = t.Id.ToString() })
				.ToList();
		}

		public User PrepareUserEntity(IGroupRepository groupRepository, IHashCalculator hashCalculator)
		{
			User user;

			if (Role == UserRole.Student)
				user = new Student
				{
					Group = groupRepository.GetGroupById(GroupID),
					IsVerified = true
				};
			else
				user = new User();

			user.Surname = Surname;
			user.Name = Name;
			user.FatherName = FatherName;
			user.Email = Email;
			user.Role = Role;
			user.PasswordHash = hashCalculator.Crypt(Pass);

			return user;
		}
    }
}