using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.DomainModel.Services;
using GraphLabs.Site.Utils;
using GraphLabs.Site.Models;

namespace GraphLabs.Site.Controllers
{
    public class GroupController : Controller
    {
        private readonly GraphLabsContext _ctx = new GraphLabsContext();
        private readonly ISystemDateService _dateService = ServiceLocator.Locator.Get<ISystemDateService>();

        //
        // GET: /Group/

        public ActionResult Index(string message)
        {
            if (!this.IsUserInRole(_ctx, UserRole.Teacher))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }

            var groups = (from g in _ctx.Groups
                          select g).ToArray()
                          .Select(t => new GroupModel(t))
                          .ToArray();

            return View(groups);
        }
        
        //
        // GET: /Group/Create

        public ActionResult Create()
        {
            if (!this.IsUserInRole(_ctx, UserRole.Teacher))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }

            Group group = new Group();
            group.FirstYear = DateTime.Today.Year;

            return View(group);
        }

        //
        // POST: /Group/Create

        [HttpPost]
        public ActionResult Create(Group group)
        {
            if (!this.IsUserInRole(_ctx, UserRole.Teacher))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }

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
            if (!this.IsUserInRole(_ctx, UserRole.Teacher))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }

            Group group = _ctx.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            GroupModel gr = new GroupModel(group);

            return View(gr);
        }

        //
        // POST: /Group/Edit/5

        [HttpPost]
        public ActionResult Edit(GroupModel gr)
        {
            if (!this.IsUserInRole(_ctx, UserRole.Teacher))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }

            if (ModelState.IsValid)
            {
                Group group = _ctx.Groups.Find(gr.Id);
                if (group == null)
                {
                    return HttpNotFound();
                }
                group.Number = gr.Number;
                group.FirstYear = gr.FirstYear;
                group.IsOpen = gr.IsOpen;
                _ctx.Entry(group).State = EntityState.Modified;
                _ctx.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gr);
        }
        
        protected override void Dispose(bool disposing)
        {
            _ctx.Dispose();
            base.Dispose(disposing);
        }
    }
}