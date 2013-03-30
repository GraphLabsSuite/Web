using System.Linq;
using System.Web.Mvc;
using GraphLabs.DataModel;

namespace GraphLabs.Site.Controllers
{
    public class PermController : Controller
    {
        private GraphLabsContext db = new GraphLabsContext();

        //
        // GET: /Perm/

        public ActionResult Index()
        {
            return View(db.Permissions.ToList());
        }
        
        //
        // GET: /Perm/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Perm/Create

        [HttpPost]
        public ActionResult Create(Permission permission)
        {
            if (ModelState.IsValid)
            {
                db.Permissions.Add(permission);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(permission);
        }
        
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}