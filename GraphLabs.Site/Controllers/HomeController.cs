using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.News;

namespace GraphLabs.Site.Controllers
{
    /// <summary> Главная </summary>
    [GLAuthorize(UserRole.Teacher, UserRole.Administrator)]
    public class HomeController : GraphLabsController
    {
        #region Зависимости

        private readonly IListModelLoader _listModelLoader;
        private readonly IEntityBasedModelLoader<NewsModel, News> _modelLoader;
        private readonly IEntityBasedModelSaver<NewsModel, News> _modelSaver;

        #endregion

        /// <summary> Главная </summary>
        public HomeController(
            IListModelLoader listModelLoader,
            IEntityBasedModelLoader<NewsModel, News> modelLoader,
            IEntityBasedModelSaver<NewsModel, News> modelSaver
            )
        {
            _listModelLoader = listModelLoader;
            _modelLoader = modelLoader;
            _modelSaver = modelSaver;
        }

        /// <summary> Главная: новости </summary>
        [AllowAnonymous]
        public ActionResult Index(string statusMessage, string statusDescription)
        {
            ViewBag.StatusMessage = statusMessage;
            ViewBag.StatusDescription = statusDescription;

            var listModel = _listModelLoader.LoadListModel<NewsListModel, NewsModel>();
            listModel.MaxNewsCount = 10;
            return View(listModel);
        }

        /// <summary> Главная: новости - редактировать </summary>
        public ActionResult Edit(long? id)
        {
            if (id.HasValue)
            {
                return View(_modelLoader.Load(id.Value));
            }

            return View();
        }

        /// <summary> Главная: новости - редактировать </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NewsModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _modelSaver.CreateOrUpdate(model);
                if (entity != null)
                {
                    return RedirectToAction("Index", new { StatusMessage = UserMessages.HomeController_Edit_Новость_успешно_опубликована_ });
                }

                ModelState.AddModelError(STD_VALIDATION_MSG_KEY, "Возникла ошибка.");
            }

            return View(model);
        }
    }
}
