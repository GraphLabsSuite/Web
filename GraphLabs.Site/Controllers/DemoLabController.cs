using GraphLabs.DomainModel;
using GraphLabs.Site.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.Site.Logic.Labs;
using System.Collections.Generic;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher, UserRole.Student)]
    public class DemoLabController : GraphLabsController
    {
        private IDemoLabEngine DemoLabEngine
        {
            get { return DependencyResolver.GetService<IDemoLabEngine>(); }
        }

        public ActionResult Index()
        {
            var model = new List<DemoLabModel>();

            foreach (var lab in DemoLabEngine.GetDemoLabs())
            {
                model.Add(new DemoLabModel(lab, DemoLabEngine.GetDemoLabVariantsByLabWorkId(lab.Id)));
            }

            if (model.Count == 0)
            {
                return View("NoDemoLabs");
            }
            else
            {
                return View(model);
            }
        }
    }
}
