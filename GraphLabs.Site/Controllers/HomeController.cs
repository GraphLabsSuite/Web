using System;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.DomainModel.Services;
using GraphLabs.Site.Models;
using GraphLabs.Site.Utils;

namespace GraphLabs.Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly GraphLabsContext _ctx = new GraphLabsContext();

        public ActionResult Index()
        {
            ViewBag.Message = "Измените этот шаблон, чтобы быстро приступить к работе над приложением ASP.NET MVC.";

            return View();
        }

        /// <summary> Начальное заполнение ViewBag </summary>
        public ActionResult Registration()
        {
            FillGroups();
            return View();
        }

        /// <summary> Обрабатываем Submit регистрации </summary>
        [HttpPost]
        public ActionResult Registration(Registration reg)
        {
            if (ModelState.IsValid)
            {
                var student = new Student
                    {
                        Login = reg.Login, 
                        PasswordHash = Hash(reg.Password), 
                        Name = reg.Name, 
                        Surname = reg.SurName, 
                        FatherName = reg.FatherName,
                        Email = reg.Email, 
                        IsVerified = false
                    };
                _ctx.Users.Add(student);
                var group = _ctx.Groups.Single(g => g.Id == reg.ID_Group);
                group.Students.Add(student);

                _ctx.SaveChanges();
                return RedirectToAction("Index");
            }

            FillGroups(reg.ID_Group);
            return View(reg);
        }

        private void FillGroups(object selectedValue = null)
        {
            var dateService = ServiceLocator.Locator.Get<ISystemDateService>();
            var groups = from g in _ctx.Groups
                         where g.IsOpen
                         orderby g.GetName(dateService)
                         select g;

            ViewBag.ID_Group = new SelectList(groups, "ID_Group", "Name", selectedValue);
        }

        //TODO: вытащить в какие-нибудь Utils
        private string Hash(string p)
        {
            return p;
        }

        public ActionResult Auth()
        {
            return View();
        }

        /// <summary> Вход </summary>
        [HttpPost]
        public ActionResult Auth(Auth log)
        {
            ViewBag.Message = "";
            if (ModelState.IsValid)
            {
                var passHash = Hash(log.Password);
                var foundUsers = from u in _ctx.Users
                                 where u.Login == log.Login && u.PasswordHash == passHash && (!(u is Student) || (u as Student).IsVerified)
                                 select u;
                
                User user;
                try
                {
                    user = foundUsers.Single();
                }
                catch (InvalidOperationException)
                {
                    ViewBag.Message = "Такая пара логин-пароль не найдена!";
                    return View(log);
                }

                FillSession(user);

                return RedirectToAction("Index");
            }

            return View(log);
        }

        private void FillSession(User user)
        {
            var guid = Guid.NewGuid();

            Session["id_user"] = user.Id;
            Session["guid"] = guid.ToString();

            // Если остались активные сессии - убьём их
            var oldSessions = _ctx.Sessions.Where(s => s.User.Id == user.Id && s.IsActive);
            foreach (var s in oldSessions)
            {
                _ctx.Sessions.Remove(s);
            }

            // Впишем новую
            var session = new Session
                {
                    CreationTime = DateTime.Now,
                    IsActive = true,
                    User = user,
                    Guid = guid,
                    IP = this.GetClientIP()
                };
            _ctx.Sessions.Add(session);
            _ctx.SaveChanges();
        }
    }
}
