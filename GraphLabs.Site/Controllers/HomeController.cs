using System;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DataModel;
using GraphLabs.Site.Models;

namespace GraphLabs.Site.Controllers
{
    public class HomeController : Controller
    {
        private GraphLabsContext db = new GraphLabsContext();

        public ActionResult Index()
        {
            ViewBag.Message = "Измените этот шаблон, чтобы быстро приступить к работе над приложением ASP.NET MVC.";

            return View();
        }

        public ActionResult Registration()
        {
            var groups = from g in db.Groups
                         where g.IsRegistrationAvailbale == true
                         where g.ID_Group >= 2
                         orderby g.Name
                         select g;
            ViewBag.ID_Group = new SelectList(groups, "ID_Group", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Registration(Registration reg)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Login = reg.Login, PasswordHash = Hash(reg.Password), Name = reg.Name, Surname = reg.SurName, FatherName = reg.FatherName, Email = reg.Email, Verify = false };
                db.Users.Add(user);
                var cur_group = from g in db.Groups
                                where g.ID_Group == reg.ID_Group
                                select g;
                Group gr = cur_group.First();
                var def_term = from t in db.Terms
                               where t.ID_Term == 1
                               select t;
                Term dt = def_term.First();
                StudyInGroup sig = new StudyInGroup { User = user, Group = gr, Term = dt };
                db.StudyInGroups.Add(sig);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var groups = from g in db.Groups
                         where g.IsRegistrationAvailbale == true
                         where g.ID_Group >= 1
                         orderby g.Name
                         select g;
            ViewBag.ID_Group = new SelectList(groups, "ID_Group", "Name", reg.ID_Group);
            return View(reg);
        }

        private string Hash(string p)
        {
            return p;
        }

        public ActionResult Auth()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Auth(Auth log)
        {
            ViewBag.Message = "";
            if (ModelState.IsValid)
            {
                String passHash = Hash(log.Password);
                var us = from u in db.Users
                         where u.Login == log.Login
                         where u.PasswordHash == passHash
                         where u.Verify == true
                         select u;
                
                User user;
                try
                {
                    user = us.First();
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
            Session["Name"] = user.Name;
            Session["Surame"] = user.Surname;
            Session["FatherName"] = user.FatherName;

            var group = from g in db.StudyInGroups
                        where g.User.ID_User == user.ID_User
                        select g.Group;
            Group gr = group.First();

            Session["ID_Group"] = gr.ID_Group;
        }
    }
}
