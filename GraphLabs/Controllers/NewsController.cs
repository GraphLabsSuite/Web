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
    public class NewsController : Controller
    {
        private GraphContext db = new GraphContext();

        //
        // GET: /News/

        public ActionResult Index(int? page)
        {
            if (!UserGroupChecking.UserIsAdminOrTeacher(this))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var newsquery = from n in db.News
                            select n;
            if (UserGroupChecking.UserIsTeacher(this))
            {
                int userID = (int)Session["UserID"];
                newsquery = newsquery.Where(n => n.UserID == userID);
            }
            newsquery = newsquery.OrderByDescending(n => n.TimeOfPublic);
            var newsquerydone = newsquery.ToList();

            var news = from n in newsquerydone
                       select new NewsPubl { NewsID = n.NewsID, Header = n.Header, Text = n.Text, Date = n.TimeOfPublic.ToShortDateString(), Author = GetAuthorByID(n.UserID) };
            
            int pageSize = 10;
            int pageIndex = (page ?? 1);

            string viewName = GetViewName(this);
            return View(viewName, news.ToPagedList(pageIndex, pageSize));
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

        private string GetViewName(Controller controller)
        {
            if (UserGroupChecking.UserIsAdmin(controller))
            {
                return "IndexAdmin";
            }
            else
            {
                return "IndexTeacher";
            }
        }

        public ActionResult Create()
        {
            if (!UserGroupChecking.UserIsAdminOrTeacher(this))
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(News news)
        {
            news.TimeOfPublic = DateTime.Now;
            news.UserID = (int)Session["UserID"];
            if (ModelState.IsValid)
            {
                db.News.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(news);
        }
 
        public ActionResult Edit(int id)
        {
            if (!UserGroupChecking.UserIsAdminOrTeacher(this))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            News news = db.News.Find(id);

            if (news == null)
            {
                return View("Error");
            }

            if (UserGroupChecking.UserIsTeacher(this))
            {
                if ((int)Session["UserID"] != news.UserID)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }

            return View(news);
        }

        [HttpPost]
        public ActionResult Edit(News news)
        {
            if (ModelState.IsValid)
            {
                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(news);
        }

        public ActionResult Delete(int id)
        {
            if (!UserGroupChecking.UserIsAdminOrTeacher(this))
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return View("Error");
            }

            if (UserGroupChecking.UserIsTeacher(this))
            {
                if ((int)Session["UserID"] != news.UserID)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }

            NewsPubl newsPubl = new NewsPubl { NewsID = news.NewsID, Header = news.Header, Text = PreparationString(news.Text), Date = news.TimeOfPublic.ToShortDateString(), Author = GetAuthorByID(news.UserID) };

            return View(newsPubl);
        }

        private string PreparationString(string source)
        {
            return Regex.Replace(source, Environment.NewLine, "<br>");
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                News news = db.News.Find(id);
                db.News.Remove(news);
                db.SaveChanges();
            }
            catch (DataException)
            {
                return RedirectToAction("Delete", new System.Web.Routing.RouteValueDictionary {
                { "id", id },  
                { "saveChangesError", true } });
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Delete", new System.Web.Routing.RouteValueDictionary {
                { "id", id },  
                { "saveChangesError", true } });
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}