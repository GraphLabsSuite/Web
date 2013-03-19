using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GraphLabs.DataModel;
using GL.Models;

namespace GL.Controllers
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
            ViewBag.GroupID = new SelectList(groups, "GroupID", "Name", reg.ID_Group);
            return View(reg);
        }

        private string Hash(string p)
        {
            return p;
        }
    }
}
