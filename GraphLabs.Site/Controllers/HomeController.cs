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
        private INewsRepository NewsRepository
        {
            get { return DependencyResolver.GetService<INewsRepository>(); }
        }

        private INewsManager NewsManager
        {
            get { return DependencyResolver.GetService<INewsManager>(); }
        }

        private IAuthenticationSavingService AuthService
        {
            get { return DependencyResolver.GetService<IAuthenticationSavingService>(); }
        }

        /// <summary> Главная: новости </summary>
        [AllowAnonymous]
        public ActionResult Index(string statusMessage, string statusDescription)
        {
            ViewBag.StatusMessage = statusMessage;
            ViewBag.StatusDescription = statusDescription;

            var news = NewsRepository.GetNewsSortedByDate(10).Select(n => new NewsModel(n));
            return View(news.ToArray());
        }

        /// <summary> Главная: новости - редактировать </summary>
        public ActionResult Edit(long? id)
        {
            if (id.HasValue)
            {
                var newsToEdit = NewsRepository.GetById(id.Value);
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
                var sessionInfo = AuthService.GetSessionInfo();
                var success = NewsManager.CreateOrEditNews(model.Id, model.Title, model.Text, sessionInfo.Email);
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
