using GraphLabs.Site.Controllers.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.DemoLabs;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher, UserRole.Student)]
    public class DemoLabController : GraphLabsController
    {

        #region Зависимости

        private readonly IListModelLoader _listModelLoader;

        #endregion

        public DemoLabController(IListModelLoader listModelLoader)
        {
            _listModelLoader = listModelLoader;
        }

        public ActionResult Index()
        {
            var model = _listModelLoader.LoadListModel<DemoLabListModel, DemoLabModel>();

            return View(model);
        }
    }
}
