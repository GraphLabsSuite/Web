using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.Site.Utils;
using System.IO;

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

        public ActionResult XAP()
        {
            this.AllowAnonymous(_ctx);
            Task task = _ctx.Tasks.First();
            byte[] x = task.Xap;
            
            return View(x);
        }

        public byte[] data()
        {
            this.AllowAnonymous(_ctx);
            Task task = _ctx.Tasks.First();
            byte[] x = task.Xap;

            return x;
        }
    }
}
