using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.DomainModel.Services;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Logic.Security;
using GraphLabs.Site.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Teacher, UserRole.Administrator)]
    public class UserController : GraphLabsController
    {
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

		private IHashCalculator _hashCalculator
		{
			get { return DependencyResolver.GetService<IHashCalculator>(); }
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

		#region Редактирование пользователя

		[HttpGet]
		public ActionResult Edit(long id = 0)
        {
			var user = _userRepository.GetUserById(id);

			// Преподаватели могут редактировать только студентов
            if (user.Role != UserRole.Student && !User.IsInRole(UserRole.Administrator))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }

            var model = new UserEdit(user);

			model.FillGroupList(_groupRepository.GetOpenGroups(), DateService);

            return View(model);
        }

        [HttpPost]
        public ActionResult Verify(UserEdit user)
        {
            ModelState.Remove("IsVerified");

			_userRepository.VerifyStudent(user.Id);

			user.IsVerified = true;
			user.FillGroupList(_groupRepository.GetOpenGroups(), DateService);

            return View("~/Views/User/Edit.cshtml", user);
        }

        [HttpPost]
        public ActionResult Dismiss(UserEdit user)
        {
            ModelState.Remove("IsDismissed");

			_userRepository.DismissStudent(user.Id);

			user.IsDismissed = true;
			user.FillGroupList(_groupRepository.GetOpenGroups(), DateService);

			return View("~/Views/User/Edit.cshtml", user);
        }
		
        [HttpPost]
        public ActionResult Save(UserEdit model)
        {
            if (model.Role != UserRole.Student && !User.IsInRole(UserRole.Administrator))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }
            
            if (ModelState.IsValid)
            {
				User user = _userRepository.GetUserById(model.Id);
				user = model.PrepareEntity(user, _groupRepository);
				if (_userRepository.TryEditUser(user))
				{
					return RedirectToAction("Index");
				}
            }

			model.FillGroupList(_groupRepository.GetOpenGroups(), DateService);
			ViewBag.Message = "Не удалось сохранить пользователя, попробуйте указать другие данные";
			return View("~/Views/User/Edit.cshtml", model);
        }

		#endregion
		
		#region Создание пользователя

		[GLAuthorize(UserRole.Administrator)]
        public ActionResult Create()
        {
            var model = new UserCreate();

			model.FillGroupList(_groupRepository.GetOpenGroups(), DateService);

            return View(model);
        }

        [GLAuthorize(UserRole.Administrator)]
        [HttpPost]
        public ActionResult Create(UserCreate model)
        {
            if (ModelState.IsValid)
            {
				User user = model.PrepareUserEntity(_groupRepository, _hashCalculator);

				if (_userRepository.TrySaveUser(user))
				{
					return RedirectToAction("Index");
				}
				ViewBag.Message = "Пользователь с таким Email существует";
            }

			model.FillGroupList(_groupRepository.GetOpenGroups(), DateService);

			return View(model);
        }

		#endregion
    }
}