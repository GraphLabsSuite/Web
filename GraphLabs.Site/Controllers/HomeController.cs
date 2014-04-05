using System.Web.Mvc;

namespace GraphLabs.Site.Controllers
{
    /// <summary> Главная </summary>
    [AllowAnonymous]
    public class HomeController : GraphLabsController
    {
        /// <summary> Главная: новости </summary>
        public ActionResult Index(string statusMessage, string statusDescription)
        {
            ViewBag.StatusMessage = statusMessage;
            ViewBag.StatusDescription = statusDescription;
            return View();
        }
    }
}
