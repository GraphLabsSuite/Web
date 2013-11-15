using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GraphLabs.DomainModel.Utils;
using GraphLabs.Site.Models;
using GraphLabs.Site.Utils;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;

namespace GraphLabs.Site.Controllers
{
    /// <summary> Контроллер для вариантов заданий </summary>
    public class TaskVariantController : Controller
    {
        private readonly GraphLabsContext _ctx = new GraphLabsContext();

        /// <summary> Начальная отрисовка списка </summary>
        public ActionResult Index(string message, long taskId)
        {
            if (!this.IsUserInRole(_ctx, UserRole.Teacher))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }

            var task = _ctx.Tasks.Find(taskId);
            if (task == null)
                return RedirectToAction("Index", "Task");

            var variants = (from variant in _ctx.TaskVariants
                            where variant.Task.Id == taskId
                            select variant).ToArray()
                                           .Select(t => new TaskVariantModel(t))
                                           .ToArray();

            ViewBag.Message = message;
            ViewBag.TaskName = task.Name;

            return View(variants);
        }


        #region EditVariant
        
        /// <summary> Начальная отрисовка формы редактирования </summary>
        public ActionResult EditVariant(long? variantId, long taskId, string message)
        {
            if (!this.IsUserInRole(_ctx, UserRole.Teacher))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }
            if (variantId != null)
            {
                var variant = _ctx.TaskVariants.Find(variantId);
            }
            ViewBag.Message = message;
            ViewBag.TaskId = taskId;

            return View();
        }

        #endregion
    }
}
