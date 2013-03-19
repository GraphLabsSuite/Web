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
    public class TermController : Controller
    {
        private GraphLabsContext db = new GraphLabsContext();

        //
        // GET: /Term/

        public ActionResult Index()
        {
            return View(db.Terms.ToList());
        }

        //
        // GET: /Term/Details/5

        public ActionResult Details(long id = 0)
        {
            Term term = db.Terms.Find(id);
            if (term == null)
            {
                return HttpNotFound();
            }
            return View(term);
        }

        //
        // GET: /Term/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Term/Create

        [HttpPost]
        public ActionResult Create(Term term)
        {
            if (ModelState.IsValid)
            {
                db.Terms.Add(term);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(term);
        }

        //
        // GET: /Term/Edit/5

        public ActionResult Edit(long id = 0)
        {
            Term term = db.Terms.Find(id);
            if (term == null)
            {
                return HttpNotFound();
            }
            return View(term);
        }

        //
        // POST: /Term/Edit/5

        [HttpPost]
        public ActionResult Edit(Term term)
        {
            if (ModelState.IsValid)
            {
                db.Entry(term).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(term);
        }

        //
        // GET: /Term/Delete/5

        public ActionResult Delete(long id = 0)
        {
            Term term = db.Terms.Find(id);
            if (term == null)
            {
                return HttpNotFound();
            }
            return View(term);
        }

        //
        // POST: /Term/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            Term term = db.Terms.Find(id);
            db.Terms.Remove(term);
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