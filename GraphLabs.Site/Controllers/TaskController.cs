using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GraphLabs.Site.Utils;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;

namespace GraphLabs.Site.Controllers
{
    /// <summary> Контроллер для заданий </summary>
    public class TaskController : Controller
    {
        private readonly GraphLabsContext _ctx = new GraphLabsContext();

        //
        // GET: /Tasks/
        /// <summary> Начальная отрисовка </summary>
        public ActionResult Index(string message)
        {
            if (!this.IsUserInRole(_ctx, UserRole.Teacher))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }

            var tasks = (from task in _ctx.Tasks
                         select task)
                        .ToArray();

            ViewBag.Message = message;

            return View(tasks);
        }

        #region Upload

        /// <summary> Начальная отрисовка формы загрузки </summary>
        public ActionResult Upload(string message)
        {
            if (!this.IsUserInRole(_ctx, UserRole.Teacher))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }

            ViewBag.Message = message;

            return View();
        }

        /// <summary> Загружаем задание </summary>
        [HttpPost]
        public ActionResult Upload()
        {
            if (!this.IsUserInRole(_ctx, UserRole.Teacher))
            {
                return RedirectToAction("Index", "Home", new { Message = UserMessages.ACCES_DENIED });
            }

            // Verify that the user selected a file
            if (Request.Files.Count == 1 && Request.Files[0] != null && Request.Files[0].ContentLength > 0)
            {
                var file = Request.Files[0];

                var newTask = _ctx.Tasks.CreateFromXap(file.InputStream);
                if (newTask == null)
                    return RedirectToAction("Upload", "Task", new { Message = UserMessages.UPLOAD_ERROR });

                var sameTasks = _ctx.Tasks.Where(t => t.Name == newTask.Name && t.Version == newTask.Version);
                if (sameTasks.Any())
                {
                    return RedirectToAction("Upload", "Task", new { Message = UserMessages.TASK_EXISTS });
                }

                try
                {
                    _ctx.Tasks.Add(newTask);
                    _ctx.SaveChanges();
                }
                catch (Exception)
                {
                    return RedirectToAction("Index", "Task", new { Message = UserMessages.UPLOAD_ERROR });
                }
                

                return RedirectToAction("Edit", "Task", new { Id = newTask.Id });
            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("Upload", "Task", new { Message = UserMessages.UPLOAD_FILE_NOT_SPECIFIED }); 
        }

        #endregion
    }
}
