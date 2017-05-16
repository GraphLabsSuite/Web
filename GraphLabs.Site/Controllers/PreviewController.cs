using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GraphLabs.Site.Models.Preview;
using GraphLabs.Site.Models.LabExecution;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher, UserRole.Student)]
    public class PreviewController : GraphLabsController
    {
        #region Зависимости
        private readonly ITaskVariantPreviewModelLoader _taskVariantPreviewModelLoader;
        #endregion

        public PreviewController(ITaskVariantPreviewModelLoader taskVariantPreviewModelLoader)
        {
            _taskVariantPreviewModelLoader = taskVariantPreviewModelLoader;
        }


        /// <summary> Может содержать список всех модулей для перехода в модуль и превью конкретного варианта модуля </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary> Превью конкретного модуля </summary>
        public ActionResult TaskVariant(int taskId)
        {
            return View(_taskVariantPreviewModelLoader.Load(taskId));
        }

    }
}
