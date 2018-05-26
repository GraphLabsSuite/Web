using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Site.Models.Account
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

		public List<SelectListItem> GroupList { get; private set; }
		public void FillGroupList(Group[] groups)
		{
			GroupList = groups
				.Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() })
				.ToList();
		}

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

			if (user.Role == UserRole.Student)
			{
				var student = (Student)user;
				GroupID = student.Group.Id;
				IsVerified = student.IsVerified;
				IsDismissed = student.IsDismissed;
			}
        }

		public User PrepareEntity(User user, IGroupRepository groupRepository)
		{
			user.Name = Name;
			user.Surname = Surname;
			user.FatherName = FatherName;
			user.Role = Role;

			if (user.Role == UserRole.Student)
			{
				if (!IsVerified.HasValue || !IsDismissed.HasValue || !GroupID.HasValue)
					throw new InvalidOperationException();

				((Student)user).IsVerified = IsVerified.Value;
				((Student)user).IsDismissed = IsDismissed.Value;
				((Student)user).Group = groupRepository.GetGroupById(GroupID.Value);
			}

			return user;
		}
    }
}