using GraphLabs.Site.Controllers.Attributes;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.Site.Utils;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.Startpage;
using GraphLabs.Site.Models.Startpage.Edit;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
    public class StartpageController : Controller
    {
        private IEditLabScheduleModelLoader _editModelLoader;
        private readonly IListModelLoader _listModelLoader;
        private IEntityBasedModelLoader<LabStartpageModel, AbstractLabSchedule> _modelLoader;
        private IEntityBasedModelSaver<EditLabScheduleModelBase, AbstractLabSchedule> _modelSaver;

        public StartpageController(
            IListModelLoader listModelLoader,
            IEntityBasedModelSaver<EditLabScheduleModelBase, AbstractLabSchedule> modelSaver,
            IEntityBasedModelLoader<LabStartpageModel, AbstractLabSchedule> modelLoader,
            IEditLabScheduleModelLoader editModelLoader)
        {
            _listModelLoader = listModelLoader;
            _modelSaver = modelSaver;
            _modelLoader = modelLoader;
            _editModelLoader = editModelLoader;
        }

        public ActionResult Index(string message)
        {
            ViewBag.Message = message;
            var model = _listModelLoader.LoadListModel<LabStartpageListModel, LabStartpageModel>();
            return View(model);
        }
    }
}
