using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DataModel;
using GraphLabs.Site.Models;

namespace GraphLabs.Site.Controllers
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
            GroupPermissions gp = new GroupPermissions { ID_Group = id, Group_Name = group.Name, Permissions = new List<PermChoose>() };
            foreach (var item in perms)
            {
                if (group.Permission.Contains(item))
                {
                    gp.Permissions.Add(new PermChoose { ID_Permission = item.ID_Permission, Enable = true, Name = item.Description });
                }
                else
                {
                    gp.Permissions.Add(new PermChoose { ID_Permission=item.ID_Permission, Enable = false, Name = item.Description });
                }
            }
            
            return View(gp);
        }
        
        [HttpPost]
        public ActionResult Perm(GroupPermissions gp)
        {
            Group group = db.Groups.Find(gp.ID_Group);
            if (group == null)
            {
                return HttpNotFound();
            }

            group.Permission.Clear();

            foreach (var item in gp.Permissions)
            {
                if (item.Enable)
                {
                    Permission p = db.Permissions.Find(item.ID_Permission);
                    group.Permission.Add(p);
                }
            }

            db.Entry(group).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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