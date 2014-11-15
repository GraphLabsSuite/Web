using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Logic.Tasks;
using GraphLabs.Site.Models;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Controllers
{
    /// <summary> Контроллер для заданий </summary>
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
    public class TaskController : GraphLabsController
    {
        private ITaskRepository TaskRepository
        {
            get { return DependencyResolver.GetService<ITaskRepository>(); }
        }

        private ITaskManager TaskManager
        {
            get { return DependencyResolver.GetService<ITaskManager>(); }
        }

        //
        // GET: /Tasks/
        /// <summary> Начальная отрисовка списка </summary>
        public ActionResult Index()
        {
            var tasks = TaskRepository.GetAllTasks()
                .Select(t => new TaskModel(t, true))
                .ToArray();
            
            return View(tasks);
        }

        #region UploadTask

        /// <summary> Начальная отрисовка формы загрузки </summary>
        public ActionResult UploadTask(string errorMessage)
        {
            ViewBag.ErrorMessage = errorMessage;
            return View();
        }

        /// <summary> Загружаем задание </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadTask(HttpPostedFileBase xap)
        {
            // Verify that the user selected a file
            if (xap != null && xap.ContentLength > 0)
            {
                Task newTask;
                try
                {
                    using (TransactionManager.BeginTransaction())
                    {
                        newTask = TaskManager.UploadTask(xap.InputStream);
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("UploadTask", "Task", new { ErrorMessage = UserMessages.UPLOAD_ERROR });
                }

                if (newTask == null)
                    return RedirectToAction("UploadTask", "Task", new { ErrorMessage = UserMessages.TASK_EXISTS });

                return RedirectToAction("EditTask", "Task", new { Id = newTask.Id, StatusMessage = UserMessages.TaskController_UploadTask_Задание_успешно_загружено });
            }

            // redirect back to the index action to show the form once again
            return RedirectToAction("UploadTask", "Task", new { ErrorMessage = UserMessages.UPLOAD_FILE_NOT_SPECIFIED }); 
        }

        #endregion


        #region EditTask
        
        //
        // GET: /Tasks/Edit
        /// <summary> Начальная отрисовка формы редактирования </summary>
        //TODO: объединить statusMessage и errorMessage в одну структуру, и скооперировать с _StatusMessagePartial
        public ActionResult EditTask(long id, string statusMessage, string errorMessage)
        {
            var task = TaskRepository.FindById(id);
            
            if (task == null)
                return InvokeHttp404(HttpContext);

            ViewBag.StatusMessage = statusMessage;
            ViewBag.ErrorMessage = errorMessage;

            return View(new TaskModel(task));
        }

        //
        // POST: /Tasks/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTask(TaskModel model)
        {
            var task = TaskRepository.FindById(model.Id);
            if (task == null)
                return InvokeHttp404(HttpContext);
            try
            {
                TaskManager.UpdateNote(task, model.Note);
                
                return RedirectToAction("EditTask", new { Id = model.Id, StatusMessage = UserMessages.EDIT_COMPLETE });
            }
            catch (Exception)
            {
                throw;
            }
        }

        //
        // POST: /Tasks/EditVariantGenerator
        /// <summary> Начальная отрисовка формы редактирования </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditVariantGenerator(HttpPostedFileBase newGenerator, TaskModel model,
            string upload, string delete)
        {
            var task = TaskRepository.FindById(model.Id);
                    if (task == null)
                        return InvokeHttp404(HttpContext);

            if (!string.IsNullOrEmpty(upload))
            {
                // Проверим, что вообще есть, что загружать
                if (newGenerator != null && newGenerator.ContentLength > 0)
                {
                    try
                    {
                        TaskManager.SetGenerator(task, newGenerator.InputStream);
                        return RedirectToAction("EditTask", new { Id = model.Id, StatusMessage = UserMessages.EDIT_COMPLETE });
                    }
                    catch (Exception)
                    {
                        return RedirectToAction("EditTask", new { Id = model.Id, ErrorMessage = UserMessages.UPLOAD_ERROR });
                    }
                }

                return RedirectToAction("EditTask", new { Id = model.Id, ErrorMessage = UserMessages.UPLOAD_FILE_NOT_SPECIFIED });
            }

            if (!string.IsNullOrEmpty(delete))
            {
                TaskManager.RemoveGenerator(task);
                return RedirectToAction("EditTask", new { Id = model.Id, StatusMessage = UserMessages.EDIT_COMPLETE });
            }

            throw new ArgumentException("Ошибка при обработке запроса на редактирования генератора вариантов - неверный набор входных аргументов.");
        }

        #endregion
    }
}
