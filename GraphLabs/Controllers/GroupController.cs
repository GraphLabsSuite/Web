using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.Site.DAL;
using GraphLabs.Site.Models;
using PagedList;

namespace GraphLabs.Site.Controllers
{ 
    public class GroupController : Controller
    {
        private GraphContext db = new GraphContext();

        public ActionResult Index(string sortOrder, int? page)
        {
            if (! UserGroupChecking.UserIsAdminOrTeacher(this))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "Name desc" : "";
            ViewBag.YearSortParam = sortOrder == "Year" ? "Year desc" : "Year";
            ViewBag.ActiveSortParam = sortOrder == "IsActive" ? "IsActive desc" : "IsActive";
            
            var groups = from s in db.Groups
                         where s.GroupID > 1
                         select s;
            if (! UserGroupChecking.UserIsAdmin(this))
            {
                groups = groups.Where(s => s.GroupID > 2);
            }
            
            switch (sortOrder)
            {
                case "Name desc":
                    groups = groups.OrderByDescending(s => s.Name);
                    break;
                case "Year":
                    groups = groups.OrderBy(s => s.Year);
                    break;
                case "Year desc":
                    groups = groups.OrderByDescending(s => s.Year);
                    break;
                case "IsActive":
                    groups = groups.OrderBy(s => s.IsActive);
                    break;
                case "IsActive desc":
                    groups = groups.OrderByDescending(s => s.IsActive);
                    break;
                default:
                    groups = groups.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 10;
            int pageIndex = (page ?? 1);

            return View(groups.ToPagedList(pageIndex, pageSize));
        }
        
        public ActionResult Create()
        {
            if (! UserGroupChecking.UserIsAdminOrTeacher(this))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            return View();
        } 
        
        [HttpPost]
        public ActionResult Create(Group group)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Groups.Add(group);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(group);
        }
         
        public ActionResult Edit(int id)
        {
            if (! UserGroupChecking.UserIsAdminOrTeacher(this))
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            if (id == 1)
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            if ((id == 2) && (! UserGroupChecking.UserIsTeacher(this)))
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            Group group = db.Groups.Find(id);
            return View(group);
        }

        [HttpPost]
        public ActionResult Edit(Group group)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(group).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(group);
        }

        public ActionResult Delete(int id, bool? saveChangesError)
        {
            if (!this.UserIsAdminOrTeacher())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if ((id == 1) || (id == 2))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Unable to save changes. Try again, and if the problem persists see your system administrator.";
            } 
            Group group = db.Groups.Find(id);
            return View(group);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Group group = db.Groups.Find(id);
                db.Groups.Remove(group);
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