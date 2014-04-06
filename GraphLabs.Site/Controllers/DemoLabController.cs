using GraphLabs.DomainModel;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Logic.Labs;
using GraphLabs.Site.Models;
using System.Collections.Generic;
using System.Web.Mvc;

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

            return ViewReturn(model);
        }

        /// <summary> Вспомогательная функция, в зависимости от модели определяющая какой View отобразить  </summary>
        private ActionResult ViewReturn(List<DemoLabModel> model)
        {
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
