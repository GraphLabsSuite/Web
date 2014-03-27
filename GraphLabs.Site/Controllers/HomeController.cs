using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.Site.Utils;

namespace GraphLabs.Site.Controllers
{
    public class HomeController : GraphLabsController
    {
        private readonly GraphLabsContext _ctx = new GraphLabsContext();

        public bool IsAuthenticated = true;

        [AllowAnonymous]
        public ActionResult Index(string message)
        {
            //this.AllowAnonymous();

            if (!string.IsNullOrWhiteSpace(message))
            {
                ViewBag.Message = message;
            }
            else
            {
                ViewBag.Message = "Тут будут новости.";
            }

            return View();
        }
    }
}
