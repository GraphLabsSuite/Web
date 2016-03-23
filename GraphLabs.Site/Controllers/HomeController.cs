using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Models;

namespace GraphLabs.Site.Controllers
{
    /// <summary> Главная </summary>
    [GLAuthorize(UserRole.Teacher, UserRole.Administrator)]
    public class HomeController : GraphLabsController
    {
        #region Зависимости

        private readonly IGraphLabsContext _newsContext;
        private readonly IAuthenticationSavingService _authSavingService;

        #endregion

        /// <summary> Главная </summary>
        public HomeController(
            IGraphLabsContext newsContext,
            IAuthenticationSavingService authSavingService
            )
        {
            _newsContext = newsContext;
            _authSavingService = authSavingService;
        }

        /// <summary> Главная: новости </summary>
        [AllowAnonymous]
        public ActionResult Index(string statusMessage, string statusDescription)
        {
            ViewBag.StatusMessage = statusMessage;
            ViewBag.StatusDescription = statusDescription;

            return View(new NewsListModel(_newsContext).GetNewsSortedByDate(10));
        }

        /// <summary> Главная: новости - редактировать </summary>
        public ActionResult Edit(long? id)
        {
            if (id.HasValue)
            {
                return View(new NewsListModel(_newsContext).GetById(id.Value));
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
                var success = new NewsListModel(_newsContext).CreateOrEditNews(model.Id, model.Title, model.Text, sessionInfo.Email);
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
