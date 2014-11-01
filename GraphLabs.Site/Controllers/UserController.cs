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
		
		// TODO не переписан
        [HttpPost]
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

		#endregion

		private ActionResult ReturnWithMessage(UserEdit user, string mes = "")
        {
            ViewBag.Message = mes;

            FillGroups(user.GroupID);
            return View(user);
        }

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

		private void FillGroups(object selectedValue = null)
        {
			var groups = _groupRepository.GetOpenGroups()
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