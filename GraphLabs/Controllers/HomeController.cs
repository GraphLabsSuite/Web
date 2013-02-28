using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.Site.DAL;
using GraphLabs.Site.Models;
using PagedList;
using System.Text.RegularExpressions;

namespace GraphLabs.Site.Controllers
{
    public class HomeController : Controller
    {
        private GraphContext db = new GraphContext();

        public ActionResult Index(int? page)
        {
            var newsquery = (from n in db.News 
                             orderby n.TimeOfPublic descending
                             select n).ToList();
            var news = from n in newsquery
                       select new NewsPubl { NewsID = n.NewsID, Header = n.Header, Text = PreparationString(n.Text), Date = n.TimeOfPublic.ToShortDateString(), Author = GetAuthorByID(n.UserID) };
            int pageSize = 5;
            int pageIndex = (page ?? 1);

            return View(news.ToPagedList(pageIndex, pageSize));
        }

        private string PreparationString(string source)
        {
            return Regex.Replace(source, Environment.NewLine, "<br>");
        }

        private string GetAuthorByID(int ID)
        {
            User user = db.Users.Find(ID);

            if (user == null)
            {
                return "Неизвестный автор";
            }
            else
            {
                return user.SurName + " " + user.Name;
            }
        }

        public ViewResult AccessDenied()
        {
            return View();
        }
                       
        public ActionResult Registration()
        {
            var GroupQry = from g in db.Groups
                           where g.IsActive == true
                           where g.GroupID >= 2
                           orderby g.Name
                           select g;
            ViewBag.GroupID = new SelectList(GroupQry, "GroupID", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Registration(Registration registration)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Login = registration.Login, Password = Hash(registration.Password), Verify = false, Name = registration.Name, SurName = registration.SurName, FatherName = registration.FatherName, Email = registration.Email, GroupID = registration.GroupID };
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var GroupQry = from g in db.Groups
                           where g.IsActive == true
                           where g.GroupID >= 2
                           orderby g.Name
                           select g;
            ViewBag.GroupID = new SelectList(GroupQry, "GroupID", "Name", registration.GroupID);
            return View(registration);
        }

        public ActionResult Login()
        {
            if (UserGroupChecking.UserNotLogged(this))
            {
                return View();
            }
            else
            {
                return RedirectToAction("AccessDenied");
            }
        }

        [HttpPost]
        public ActionResult Login(LogOn logon)
        {
            if (ModelState.IsValid)
            {
                string hash = Hash(logon.Password);
                var user = from u in db.Users
                           where u.Login == logon.Login
                           where u.Password == hash
                           where u.Verify == true
                           select u;

                User us;
                try
                {
                    us = user.First();
                }
                catch (InvalidOperationException)
                {
                    ViewBag.ErrorMessage = "Такая пара логин-пароль не найдена";
                    return View(logon);
                }
                
                Session.Add("UserID", us.UserID);
                Session.Add("GroupID", us.GroupID);
                Session.Add("Name", us.Name);
                Session.Add("Surname", us.SurName);
                Session.Add("Fathername", us.FatherName);

                ClosePreviousSessionInDB(us.UserID);
                RegisterSession(us.UserID);

                return RedirectToAction("Index");
            }

            return View(logon);
        }

        private string Hash(string pass)
        {
            return pass + "qwe";
        }

        private void ClosePreviousSessionInDB(int UserID)
        {
            var session = from s in db.Sessions
                          where s.UserID == UserID
                          where s.IsActive == true
                          select s;
            foreach (var result in session)
            {
                result.IsActive = false;
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();
            }

        }

        private void RegisterSession(int UserID)
        {
            UserSession currentSession = SetSessionParam(UserID);
            SaveSessionParamToDB(currentSession);
        }

        private UserSession SetSessionParam(int UID)
        {
            return new UserSession { UserID = UID, IsActive = true, TimeStart = DateTime.Now };
        }

        private void SaveSessionParamToDB(UserSession us)
        {
            db.Sessions.Add(us);
            db.SaveChanges();
        }

        public ActionResult Logoff()
        {
            ClosePreviousSessionInDB((int)Session["UserID"]);

            Session["UserID"] = null;
            Session["GroupID"] = null;
            Session["Name"] = null;
            Session["Surname"] = null;
            Session["Fathername"] = null;

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
