using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GraphLabs.DataModel;

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