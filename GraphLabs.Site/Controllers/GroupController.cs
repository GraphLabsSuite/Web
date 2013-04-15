using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.DomainModel.Services;
using GraphLabs.Site.Models;
using PagedList;

namespace GraphLabs.Site.Controllers
{
    public class GroupController : Controller
    {
        private readonly GraphLabsContext _ctx = new GraphLabsContext();
        private readonly ISystemDateService _dateService = ServiceLocator.Locator.Get<ISystemDateService>();

        //
        // GET: /Group/

        public ActionResult Index(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = string.IsNullOrEmpty(sortOrder) ? "Name desc" : "";
            ViewBag.AvailableSortParam = sortOrder == "Available" ? "Available desc" : "Available";

            var groups = from g in _ctx.Groups
                         select g;

            switch (sortOrder)
            {
                case "Name desc":
                    groups = groups.OrderByDescending(g => g.GetName(_dateService));
                    break;
                case "Available":
                    groups = groups.OrderBy(g => g.IsOpen);
                    break;
                case "Available desc":
                    groups = groups.OrderByDescending(g => g.IsOpen);
                    break;
                default:
                    groups = groups.OrderBy(g => g.GetName(_dateService));
                    break;
            }

            int pageSize = 15;
            int pageIndex = (page ?? 1);
            
            return View(groups.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: /Group/Details/5

        public ActionResult Details(long id = 0)
        {
            Group group = _ctx.Groups.Find(id);
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
                _ctx.Groups.Add(group);
                _ctx.SaveChanges();
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
            Group group = _ctx.Groups.Find(id);
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
                _ctx.Entry(group).State = EntityState.Modified;
                _ctx.SaveChanges();
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
            _ctx.Dispose();
            base.Dispose(disposing);
        }
    }
}