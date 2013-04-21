using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.Site.Utils;

namespace GraphLabs.Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly GraphLabsContext _ctx = new GraphLabsContext();

        public bool IsAuthenticated = true;

        public ActionResult Index()
        {
            this.AllowAnonymous(_ctx);

            ViewBag.Message = "Тут будут новости";

            return View();
        }
    }
}
