using System.Data;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DataModel;
using GraphLabs.Site.Models;
using PagedList;

namespace GraphLabs.Site.Controllers
{
    public class UserController : Controller
    {
        private GraphLabsContext db = new GraphLabsContext();

        //
        // GET: /User/

        public ActionResult Index(string sortOrder, bool? displayUnverify, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentUnverify = (displayUnverify ?? true);
            ViewBag.SurNameSortParam = string.IsNullOrEmpty(sortOrder) ? "SurName desc" : "";
            ViewBag.VerifySortParam = sortOrder == "Verify" ? "Verify desc" : "Verify";
            ViewBag.DisplayUnverify = (displayUnverify ?? true) ? false : true;

            var users = from g in db.Users
                         select g;

            System.Collections.Generic.List<GraphLabs.Site.Models.DispUser> us = new System.Collections.Generic.List<Models.DispUser>();
            
            switch (sortOrder)
            {
                case "SurName desc":
                    users = users.OrderByDescending(g => g.Surname);
                    break;
                case "Verify":
                    users = users.OrderBy(g => g.Verify);
                    break;
                case "Verify desc":
                    users = users.OrderByDescending(g => g.Verify);
                    break;
                default:
                    users = users.OrderBy(g => g.Surname);
                    break;
            }

            if (displayUnverify ?? true)
            {
                users = users.Where(g => g.Verify == false);
            }

            foreach (var item in users)
            {
                //string gr = (from g in db.StudyInGroups
                             
                             //select g.Group.Name).ToList()[0];
                string gr;
                gr = item.StudyInGroups.First().Group.Name;
                us.Add(new GraphLabs.Site.Models.DispUser { ID = item.ID_User, Email = item.Email, FatherName = item.FatherName, Name = item.Name, Surname = item.Surname, Verify = item.Verify, Group = gr });
            }

            int pageSize = 15;
            int pageIndex = (page ?? 1);

            return View(us.ToPagedList(pageIndex, pageSize));
        }
        
        //
        // GET: /User/Edit/5

        public ActionResult Edit(long id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(long id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            User user = db.Users.Find(id);
            StudyInGroup sig = user.StudyInGroups.First();
            db.Users.Remove(user);
            db.StudyInGroups.Remove(sig);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}