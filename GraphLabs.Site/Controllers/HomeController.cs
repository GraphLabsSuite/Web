using System.Linq;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Logic;
using GraphLabs.Site.Models;

namespace GraphLabs.Site.Controllers
{
    /// <summary> Главная </summary>
    [GLAuthorize(UserRole.Teacher, UserRole.Administrator)]
    public class HomeController : GraphLabsController
    {
        #region Зависимости

        private readonly INewsRepository _newsRepository;
        private readonly INewsManager _newsManager;
        private readonly IAuthenticationSavingService _authSavingService;

        #endregion

        /// <summary> Главная </summary>
        public HomeController(
            INewsRepository newsRepository,
            INewsManager newsManager,
            IAuthenticationSavingService authSavingService
            )
        {
            _newsRepository = newsRepository;
            _newsManager = newsManager;
            _authSavingService = authSavingService;
        }

        /// <summary> Главная: новости </summary>
        [AllowAnonymous]
        public ActionResult Index(string statusMessage, string statusDescription)
        {
            ViewBag.StatusMessage = statusMessage;
            ViewBag.StatusDescription = statusDescription;

            var news = _newsRepository.GetNewsSortedByDate(10).Select(n => new NewsModel(n));
            return View(news.ToArray());
        }

        /// <summary> Главная: новости - редактировать </summary>
        public ActionResult Edit(long? id)
        {
            if (id.HasValue)
            {
                var newsToEdit = _newsRepository.GetById(id.Value);
                var model = new NewsModel(newsToEdit);
                return View(model);
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
                var sessionInfo = _authSavingService.GetSessionInfo();
                var success = _newsManager.CreateOrEditNews(model.Id, model.Title, model.Text, sessionInfo.Email);
                if (success)
                {
                    return RedirectToAction("Index", new { StatusMessage = UserMessages.HomeController_Edit_Новость_успешно_опубликована_ });
                }

                ModelState.AddModelError(STD_VALIDATION_MSG_KEY, "Возникла ошибка.");
            }

            return View(model);
        }
    }
}
