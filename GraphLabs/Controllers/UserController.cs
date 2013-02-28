using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.Site.DAL;
using GraphLabs.Site.Models;
using PagedList;

namespace GraphLabs.Site.Controllers
{ 
    public class UserController : Controller
    {
        private GraphContext db = new GraphContext();
        
        public ActionResult Index(string sortOrder, bool? VerifySortParam, int? page)
        {
            if (! UserGroupChecking.UserIsAdminOrTeacher(this))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            ViewBag.Verify = (!VerifySortParam ?? false);
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SurNameSortParam = String.IsNullOrEmpty(sortOrder) ? "SurName desc" : "";
            ViewBag.GroupSortParam = sortOrder == "Group" ? "Group desc" : "Group";
            ViewBag.IsActiveSortParam = sortOrder == "IsActive" ? "IsActive desc" : "IsActive";

            var users = from s in db.Users
                        select s;
            if ((int)Session["GroupID"] != 1)
            {
                users = users.Where(s => s.GroupID >= 3);
            }

            if (!ViewBag.Verify)
            {
                users = users.Where(s => s.Verify == false);
            }

            switch (sortOrder)
            {
                case "SurName desc":
                    users = users.OrderByDescending(s => s.SurName);
                    break;
                case "Group":
                    users = users.OrderBy(s => s.Group.Name);
                    break;
                case "Group desc":
                    users = users.OrderByDescending(s => s.Group.Name);
                    break;
                case "IsActive":
                    users = users.OrderBy(s => s.Verify);
                    break;
                case "IsActive desc":
                    users = users.OrderByDescending(s => s.Verify);
                    break;
                default:
                    users = users.OrderBy(s => s.SurName);
                    break;
            }
            int pageSize = 10;
            int pageIndex = (page ?? 1);


            return View(users.ToPagedList(pageIndex, pageSize));
        }
 
        public ActionResult Edit(int id)
        {
            if (! UserGroupChecking.UserIsAdminOrTeacher(this))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            User user = db.Users.Find(id);

            if ((user.GroupID == 1) && (! UserGroupChecking.UserIsAdmin(this)))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            UserEdit userEdit = new UserEdit { UserID = user.UserID, Name = user.Name, SurName = user.SurName, FatherName = user.FatherName, Email = user.Email, Group = user.Group, GroupID = user.GroupID, Verify = user.Verify };
            var groupQry = from g in db.Groups
                           where g.IsActive == true
                           select g;
            if (! UserGroupChecking.UserIsAdmin(this))
            {
                groupQry = groupQry.Where(g => g.GroupID > 2);
            }
            groupQry = groupQry.OrderBy(g => g.Name);

            ViewBag.GroupID = new SelectList(groupQry, "GroupID", "Name", userEdit.GroupID);
            return View(userEdit);
        }
        
        [HttpPost]
        public ActionResult Edit(UserEdit userEdit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = db.Users.Find(userEdit.UserID);
                    user.Email = userEdit.Email;
                    user.GroupID = userEdit.GroupID;
                    user.Name = userEdit.Name;
                    user.SurName = userEdit.SurName;
                    user.FatherName = userEdit.FatherName;
                    user.Verify = userEdit.Verify;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            var groupQry = from g in db.Groups
                           where g.IsActive == true
                           select g;
            if (! UserGroupChecking.UserIsAdmin(this))
            {
                groupQry = groupQry.Where(s => s.GroupID > 2);
            }
            groupQry = groupQry.OrderBy(g => g.Name);

            ViewBag.GroupID = new SelectList(groupQry, "GroupID", "Name", userEdit.GroupID);
            return View(userEdit);
        }
        
        //TODO: При удалении некорректно обрабатываются учителя

        public ActionResult Delete(int id, bool? saveChangesError)
        {
            if (! UserGroupChecking.UserIsAdminOrTeacher(this))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Unable to save changes. Try again, and if the problem persists see your system administrator.";
            } 
            User user = db.Users.Find(id);
            if (user.GroupID == 1)
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            return View(user);
        }
        
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                User user = db.Users.Find(id);
                if (user.GroupID == 1)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
                db.Users.Remove(user);
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