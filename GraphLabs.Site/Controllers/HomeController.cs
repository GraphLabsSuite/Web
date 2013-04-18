using System;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.DomainModel.Services;
using GraphLabs.Site.Models;
using GraphLabs.Site.Utils;

namespace GraphLabs.Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly GraphLabsContext _ctx = new GraphLabsContext();

        public bool IsAuthenticated = true;

        public ActionResult Index()
        {
            this.CheckAuthentication(_ctx);

            ViewBag.Message = "Тут будут новости";

            return View();
        }
    }
}
