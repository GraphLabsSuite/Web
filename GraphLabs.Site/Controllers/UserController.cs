using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Services;
using GraphLabs.Site.Models;
using PagedList;

namespace GraphLabs.Site.Controllers
{
    public class UserController : Controller
    {
        private readonly GraphLabsContext _ctx = new GraphLabsContext();
        private readonly ISystemDateService _dateService = ServiceLocator.Locator.Get<ISystemDateService>();

        //
        // GET: /User/

        public ActionResult Index(string sortOrder, bool? displayUnverify, int? page)
        {
            ViewBag.SortOrder = sortOrder;
            ViewBag.CurrentUnverify = (displayUnverify ?? true);
            ViewBag.SurNameSortParam = string.IsNullOrEmpty(sortOrder) ? "Surname desc" : "";
            ViewBag.VerifySortParam = sortOrder == "IsVerified" ? "IsVerified desc" : "IsVerified";
            ViewBag.DisplayUnverify = !(displayUnverify ?? true);

            var users = from g in _ctx.Users
                         select g;

            Expression<Func<User, bool>> verifiedCondition = u => !(u is Student) || ((Student)u).IsVerified;
            switch (sortOrder)
            {
                case "Surname desc":
                    users = users.OrderByDescending(g => g.Surname);
                    break;
                case "IsVerified":
                    users = users.OrderBy(verifiedCondition);
                    break;
                case "IsVerified desc":
                    users = users.OrderByDescending(verifiedCondition);
                    break;
                default:
                    users = users.OrderBy(g => g.Surname);
                    break;
            }

            if (displayUnverify ?? true)
            {
                users = users.Where(g => g is Student && !((Student)g).IsVerified);
            }


            var us = (from item in users select new UserModel(item, _dateService)).ToList();

            const int PAGE_SIZE = 15;
            var pageIndex = (page ?? 1);

            return View(us.ToPagedList(pageIndex, PAGE_SIZE));
        }
        
        //
        // GET: /User/Edit/5

        public ActionResult Edit(long id = 0)
        {
            User user = _ctx.Users.Find(id);
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
                _ctx.Entry(user).State = EntityState.Modified;
                _ctx.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(long id = 0)
        {
            User user = _ctx.Users.Find(id);
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
            return HttpNotFound();
            //if (!this.CheckAuthentication(UserRole.Administrator))
            //{
                
            //}

            //var user = _ctx.Users.Find(User);

            //StudyInGroup sig = user.StudyInGroups.First();
            //_ctx.Users.Remove(user);
            //_ctx.StudyInGroups.Remove(sig);
            //_ctx.SaveChanges();
            //return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _ctx.Dispose();
            base.Dispose(disposing);
        }
    }
}