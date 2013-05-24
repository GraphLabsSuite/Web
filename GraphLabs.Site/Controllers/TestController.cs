using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.Site.Utils;

namespace GraphLabs.Site.Controllers
{
    public class TestController : Controller
    {
        private readonly GraphLabsContext _ctx = new GraphLabsContext();
        //
        // GET: /Test/

        public ActionResult Index()
        {
            this.AllowAnonymous(_ctx);
            return View();
        }

    }
}
