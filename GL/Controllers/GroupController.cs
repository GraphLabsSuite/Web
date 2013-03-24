using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GraphLabs.DataModel;
using GL.Models;
using GL.Controllers.Attributes;

namespace GL.Controllers
{
    public class GroupController : Controller
    {
        private GraphLabsContext db = new GraphLabsContext();

        //
        // GET: /Group/

        public ActionResult Index()
        {
            return View(db.Groups.ToList());
        }

        //
        // GET: /Group/Details/5

        public ActionResult Details(long id = 0)
        {
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        //
        // GET: /Group/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Group/Create

        [HttpPost]
        public ActionResult Create(Group group)
        {
            if (ModelState.IsValid)
            {
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
        }

        //
        // GET: /Group/Edit/5

        public ActionResult Edit(long id = 0)
        {
            if (CheckAdministrationID(id))
            {
                return HttpNotFound();
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        //
        // POST: /Group/Edit/5

        [HttpPost]
        public ActionResult Edit(Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        public ActionResult Perm(long id = 0)
        {
            if (CheckAdministrationID(id))
            {
                return HttpNotFound();
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }

            var perms = (from p in db.Permissions select p).ToList();
            List<PermissionInGroup> perm = new List<PermissionInGroup>();
            foreach (var item in perms)
            {
                if (group.Permission.Contains(item))
                {
                    perm.Add(new PermissionInGroup { ID_Permission = item.ID_Permission, Enable = true, Name = item.Description });
                }
                else
                {
                    perm.Add(new PermissionInGroup { ID_Permission=item.ID_Permission, Enable = false, Name = item.Description });
                }
            }

            ViewBag.Name = group.Name;

            return View(perm);
        }

        [HttpPost]
        [ActionName("Perm")]
        [AcceptParameter(Name="button", Value="Стандартные права")]
        public ActionResult FillDefaultStudent(List<PermissionInGroup> perm)
        {
            return View(perm);
        }

        [HttpPost]
        [ActionName("Perm")]
        [AcceptParameter(Name = "button", Value = "Сохранить")]
        public ActionResult Perm(List<PermissionInGroup> perm)
        {
            return View(perm);
        }

        private bool CheckAdministrationID(long id)
        {
            if ((id == 1) || (id == 2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}