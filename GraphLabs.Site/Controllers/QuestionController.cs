using GraphLabs.DomainModel;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Models.Question;
using GraphLabs.Site.Models.Infrastructure;
using System.Web.Mvc;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
    public class QuestionController : GraphLabsController
    {
        private readonly IListModelLoader _listModelLoader;

        public QuestionController(IListModelLoader listModelLoader)
        {
            _listModelLoader = listModelLoader;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SelectQuestions()
        {
            var model = _listModelLoader.LoadListModel<QuestionListModel, QuestionModel>();
            return View(model);
        }
    }
}
