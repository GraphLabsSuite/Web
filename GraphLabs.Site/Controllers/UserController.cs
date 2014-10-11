using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Services;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Logic.Security;
using GraphLabs.Site.Models;
using PagedList;
using GraphLabs.Site.Utils;
using GraphLabs.DomainModel.Repositories;
using System.Collections.Generic;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Teacher, UserRole.Administrator)]
    public class UserController : GraphLabsController
    {
        private readonly GraphLabsContext _ctx = new GraphLabsContext();

		#region Зависимости

		private ISystemDateService DateService 
        {
            get { return DependencyResolver.GetService<ISystemDateService>(); }
        }

		private IUserRepository _userRepository
		{
			get { return DependencyResolver.GetService<IUserRepository>(); }
		}

		private IGroupRepository _groupRepository
		{
			get { return DependencyResolver.GetService<IGroupRepository>(); }
		}

		#endregion

		// TODO: О боже, что это?!
		public ActionResult Index()
        {
            UserIndex ui = new UserIndex();

            if (Session["Model"] != null)
            {
                ui = (UserIndex)Session["Model"];
                Session["Model"] = null;
            }

            return Index(ui);
        }

        [HttpPost]        
        public ActionResult Index(UserIndex ui)
        {
			var adminUsers = ui.Admin ? _userRepository.GetAdministrators() : new User[0];
			var teacherUsers = ui.Teacher ? _userRepository.GetTeachers() : new User[0];
			var dismissedStudents = ui.DismissStudent ? _userRepository.GetDismissedStudents() : new User[0];
			var verStudents = ui.VerStudent ? _userRepository.GetVerifiedStudents() : new User[0];
			var unverStudents = ui.UnVerStudent ? _userRepository.GetUnverifiedStudents() : new User[0];

			var userList = adminUsers
							.Concat(teacherUsers)
							.Concat(dismissedStudents)
							.Concat(verStudents)
							.Concat(unverStudents);

            var us = userList.Select(user => new UserModel(user, DateService)).ToList();

            ui.Users = us;

            Session.Add("Model", ui);

            return View(ui);
        }
		
        public ActionResult Edit(long id = 0)
        {
			var user = _userRepository.GetUserById(id);

			// Преподаватели могут редактировать только студентов
            if (user.Role != UserRole.Student && !User.IsInRole(UserRole.Administrator))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }

            var ue = new UserEdit(user);
            if (user.Role == UserRole.Student)
            {
                ue.ChangeToStudent((Student)user);             
            }

            FillGroups(ue.GroupID);

            return View(ue);
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Утвердить")]
        public ActionResult Verify(UserEdit user)
        {
            ModelState.Remove("IsVerified");
            user.IsVerified = true;
            
            Student us = (Student)_userRepository.GetUserById(user.Id);
			            
            us.IsVerified = true;
            _ctx.Entry(us).State = EntityState.Modified;
            _ctx.SaveChanges();

            FillGroups(user.GroupID);

            return View(user);
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Исключить")]
        public ActionResult Dismiss(UserEdit user)
        {
            ModelState.Remove("IsDismissed");
            user.IsDismissed = true;

			Student us = (Student)_userRepository.GetUserById(user.Id);
			
            us.IsDismissed = true;
            _ctx.Entry(us).State = EntityState.Modified;
            _ctx.SaveChanges();

            FillGroups(user.GroupID);

            return View(user);
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Удалить")]
        public ActionResult Delete(UserEdit user)
        {
            if (user.Role == UserRole.Student)
            {
				Student stud = (Student)_userRepository.GetUserById(user.Id);
                try
                {
                    _ctx.Users.Remove(stud);
                }
                catch (System.ArgumentNullException)
                {
                    return ReturnWithMessage(user, "Ошибка! Пользователь не найден в БД.");
                }
            }
            else
            {
                User us = _userRepository.GetUserById(user.Id);
                try
                {
                    _ctx.Users.Remove(us);
                }
                catch (System.ArgumentNullException)
                {
                    return ReturnWithMessage(user, "Ошибка! Пользователь не найден в БД.");
                }
            }

            _ctx.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Сохранить")]
        public ActionResult Save(UserEdit user)
        {
            if (user.Role != UserRole.Student && !User.IsInRole(UserRole.Administrator))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }
            
            if (ModelState.IsValid)
            {
                if (user.Role == UserRole.Student)
                {
					Student stud = (Student)_userRepository.GetUserById(user.Id);

                    if (stud == null)
                    {
                        return ReturnWithMessage(user, "Ошибка! Пользователь не найден в БД.");
                    }

                    stud.Group = _ctx.Groups.Find(user.GroupID);
                    stud.FatherName = user.FatherName;
                    stud.Name = user.Name;
                    stud.Surname = user.Surname;

                    _ctx.Entry(stud).State = EntityState.Modified;
                    _ctx.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
					User us = _userRepository.GetUserById(user.Id);
                    
                    if (us == null)
                    {
                        return ReturnWithMessage(user, "Ошибка! Пользователь не найден в БД.");
                    }
                    
                    us.FatherName = user.FatherName;
                    us.Surname = user.Surname;
                    us.Name = user.Name;

                    _ctx.Entry(us).State = EntityState.Modified;
                    _ctx.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return ReturnWithMessage(user);
        }

        private ActionResult ReturnWithMessage(UserEdit user, string mes = "")
        {
            ViewBag.Message = mes;

            FillGroups(user.GroupID);
            return View(user);
        }

        [GLAuthorize(UserRole.Administrator)]
        public ActionResult Create()
        {
            var user = new UserCreate();

            FillRoleList();
            FillGroups();

            return View(user);
        }

        [GLAuthorize(UserRole.Administrator)]
        [HttpPost]
        public ActionResult Create(UserCreate user)
        {
            if (ModelState.IsValid)
            {
                var HashCalculator = DependencyResolver.GetService<IHashCalculator>();
                
                if (user.Role == UserRole.Student)
                {
                    Group group = _ctx.Groups.Find(user.GroupID);
                    Student us = new Student
                    {
                        PasswordHash = HashCalculator.Crypt(user.Pass),
                        Name = user.Name,
                        Surname = user.Surname,
                        FatherName = user.FatherName,
                        Email = user.Email,
                        Role = user.Role,
                        IsVerified = true,
                        Group = group
                    };

                    try
                    {
                        _ctx.Users.Add(us);
                    }
                    catch (System.Data.SqlClient.SqlException)
                    {
                        return ReturnCreateWithMessage(user, "Пользователь с таким email уже существует!");
                    }

                    _ctx.SaveChanges();
                }
                else
                {
                    User us = new User
                    {
                        PasswordHash = HashCalculator.Crypt(user.Pass),
                        Name = user.Name,
                        Surname = user.Surname,
                        FatherName = user.FatherName,
                        Email = user.Email,
                        Role = user.Role
                    };
                    
                    try
                    {
                        _ctx.Users.Add(us);
                    }
                    catch (System.Data.SqlClient.SqlException)
                    {
                        return ReturnCreateWithMessage(user, "Пользователь с таким email уже существует!");
                    }

                    _ctx.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return ReturnCreateWithMessage(user);
        }

        private ActionResult ReturnCreateWithMessage(UserCreate user, string mes = "")
        {
            ViewBag.Message = mes;

            FillRoleList();
            FillGroups(user.GroupID);

            return View(user);
        }

        private void FillRoleList()
        {
            System.Collections.Generic.List<SelectListItem> roles = new System.Collections.Generic.List<SelectListItem>();
            roles.Add(new SelectListItem() { Text = "Администратор", Value = "4" });
            roles.Add(new SelectListItem() { Text = "Преподаватель", Value = "2" });
            roles.Add(new SelectListItem() { Text = "Студент", Value = "1" });

            ViewBag.Role = roles;
        }

        private void FillGroups(object selectedValue = null)
        {
            var groups = (from g in _ctx.Groups
                          where g.IsOpen
                          select g).ToList()
                          .Select(t => new GroupModel(t, new SystemDateService(_ctx) ))
                          .ToList();
            ViewBag.GroupID = new SelectList(groups, "Id", "Name", selectedValue);
        }
        
        protected override void Dispose(bool disposing)
        {
            _ctx.Dispose();
            base.Dispose(disposing);
        }
    }
}