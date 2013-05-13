using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Services;
using GraphLabs.Site.Models;
using PagedList;
using GraphLabs.Site.Utils;

namespace GraphLabs.Site.Controllers
{
    public class UserController : Controller
    {
        private readonly GraphLabsContext _ctx = new GraphLabsContext();
        private readonly ISystemDateService _dateService = ServiceLocator.Locator.Get<ISystemDateService>();
                
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
            this.AllowAnonymous(_ctx);

            var user = (from u in _ctx.Users
                        select u).ToList();

            System.Collections.Generic.List<User> userList = new System.Collections.Generic.List<User>();

            if (ui.Admin)
            {
                userList.AddRange(user.Where(u => u.Role == UserRole.Administrator).ToList());
            }
            if (ui.Teacher)
            {
                userList.AddRange(user.Where(u => u.Role == UserRole.Teacher).ToList());
            }
            if (ui.VerStudent)
            {
                userList.AddRange(user.Where(u => (u.Role == UserRole.Student) && (!((Student)u).IsDismissed) && ((Student)u).IsVerified).ToList());
            }
            if (ui.UnVerStudent)
            {
                userList.AddRange(user.Where(u => (u.Role == UserRole.Student) && (!((Student)u).IsDismissed) && (!((Student)u).IsVerified)).ToList());
            }
            if (ui.DismissStudent)
            {
                userList.AddRange(user.Where(u => (u.Role == UserRole.Student) && ((Student)u).IsDismissed).ToList());
            }

            var us = userList.Select(u => new UserModel(u, _dateService)).ToList();

            ui.Users = us;

            Session.Add("Model", ui);

            return View(ui);
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

        public ActionResult Create()
        {
            this.AllowAnonymous(_ctx);

            UserCreate user = new UserCreate();

            System.Collections.Generic.List<SelectListItem> roles = new System.Collections.Generic.List<SelectListItem>();
            roles.Add(new SelectListItem() { Text = "Администратор", Value = "4" });
            roles.Add(new SelectListItem() { Text = "Преподаватель", Value = "2" });

            ViewBag.Role = roles;

            return View(user);
        }

        [HttpPost]
        public ActionResult Create(UserCreate user)
        {
            this.AllowAnonymous(_ctx);

            if (ModelState.IsValid)
            {
                var salt = HashCalculator.GenerateRandomSalt();
                User us = new User
                {
                    PasswordHash = HashCalculator.GenerateSaltedHash(user.Pass, salt),
                    HashSalt = salt,
                    Name = user.Name,
                    Surname = user.Surname,
                    FatherName = user.FatherName,
                    Email = user.Email,
                    Role = user.Role
                };
                _ctx.Users.Add(us);
                _ctx.SaveChanges();
                return RedirectToAction("Index");
            }

            System.Collections.Generic.List<SelectListItem> roles = new System.Collections.Generic.List<SelectListItem>();
            roles.Add(new SelectListItem() { Text = "Администратор", Value = "4" });
            roles.Add(new SelectListItem() { Text = "Преподаватель", Value = "2" });

            ViewBag.Role = roles;

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
            //if (!this.IsAuthenticated(UserRole.Administrator))
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