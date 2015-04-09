using System.Linq;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Services;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Models.News;

namespace GraphLabs.Site.Controllers
{
    /// <summary> Главная </summary>
    [GLAuthorize(UserRole.Teacher, UserRole.Administrator)]
    public class HomeController : GraphLabsController
    {
        #region Зависимости

        private readonly INewsContext _newsContext;
        private readonly ISystemDateService _syatemDate;

        #endregion

        /// <summary> Главная </summary>
        public HomeController(
            INewsContext newsContext,
            ISystemDateService syatemDate)
        {
            _newsContext = newsContext;
            _syatemDate = syatemDate;
        }

        /// <summary> Главная: новости </summary>
        [AllowAnonymous]
        public ActionResult Index(string statusMessage, string statusDescription)
        {
            ViewBag.StatusMessage = statusMessage;
            ViewBag.StatusDescription = statusDescription;

            var model = new NewsListModel(_newsContext)
            {
                NewsToShow = 10
            };
            model.Load();

            return View(model);
        }

        /// <summary> Главная: новости - редактировать </summary>
        public ActionResult Edit(long? id)
        {
            if (id.HasValue)
            {
                var model = new NewsModel(_newsContext, _syatemDate);
                model.Load(id.Value);

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
                var success = model.Save(User.Identity.Name);
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
