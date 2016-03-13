using System.Linq;
using System.Web.Mvc;
using GraphLabs.Site.Models;
using GraphLabs.Site.Utils;
using GraphLabs.Dal.Ef;

namespace GraphLabs.Site.Controllers
{
    /// <summary> Контроллер для вариантов заданий </summary>
    public class TaskVariantController : Controller
    {
        private readonly GraphLabsContext _ctx = new GraphLabsContext();

        /// <summary> Начальная отрисовка списка </summary>
        public ActionResult Index(string message, long taskId)
        {
            //if (!this.IsUserInRole(_ctx, UserRole.Teacher))
            //{
            //    return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            //}
            //this.AllowAnonymous();

            var task = _ctx.Tasks.Find(taskId);
            if (task == null)
                return RedirectToAction("Index", "Task", new { Message = UserMessages.Задание_с_полученным_Id_не_найдено_ });

            var variants = (from variant in _ctx.TaskVariants
                            where variant.Task.Id == taskId
                            select variant).ToArray()
                                           .Select(t => new TaskVariantModel(t))
                                           .ToArray();

            ViewBag.Message = message;
            ViewBag.TaskName = task.Name;
            ViewBag.TaskId = taskId;

            return View(variants);
        }


        #region EditVariant
        
        /// <summary> Начальная отрисовка формы редактирования </summary>
        public ActionResult EditVariant(long? variantId, long taskId, string message)
        {
            //if (!this.IsUserInRole(_ctx, UserRole.Teacher))
            //{
            //    return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            //}
            //this.AllowAnonymous();

            var task = _ctx.Tasks.Find(taskId);
            if (task == null)
                return RedirectToAction("Index", new { Message = UserMessages.Задание_с_полученным_Id_не_найдено_ });

            if (variantId != null)
            {
                var variant = _ctx.TaskVariants.Find(variantId);
                if (variant == null)
                    return RedirectToAction("Index", new {Message = UserMessages.Вариант_с_полученным_Id_не_найден_});
                ViewBag.VariantNumber = variant.Number;
            }
            ViewBag.Message = message;
            ViewBag.TaskName = task.Name;
            ViewBag.TaskId = taskId;
            ViewBag.VariantId = variantId ?? -1;

            return View();
        }

        #endregion
    }
}
