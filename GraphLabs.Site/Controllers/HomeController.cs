using System.Web.Mvc;

namespace GraphLabs.Site.Controllers
{
    /// <summary> Главная </summary>
    [AllowAnonymous]
    public class HomeController : GraphLabsController
    {
        /// <summary> Главная: новости </summary>
        public ActionResult Index(string message)
        {
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
