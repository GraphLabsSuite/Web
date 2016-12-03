using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Models.AvailableLab;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher, UserRole.Student)]
    public class TestingLabController : GraphLabsController
    {

        #region Зависимости

        private readonly IListModelLoader _listModelLoader;

        #endregion

        public TestingLabController(IListModelLoader listModelLoader)
        {
            _listModelLoader = listModelLoader;
        }

        public ActionResult Index()
        {
            var model = _listModelLoader.LoadListModel<TestingLabListModel, TestingLabModel>();

            return View(model);
        }
    }
}