using GraphLabs.Site.Controllers.Attributes;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.Schedule;
using GraphLabs.Site.Models.Schedule.Edit;
using GraphLabs.Site.Utils;
using System;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
    public class StartpageController : GraphLabsController
    {
        private readonly IListModelLoader _listModelLoader;
        private readonly IEntityBasedModelSaver<EditLabScheduleModelBase, AbstractLabSchedule> _modelSaver;
        private readonly IEntityBasedModelLoader<LabScheduleModel, AbstractLabSchedule> _modelLoader;
        private readonly IEditLabScheduleModelLoader _editModelLoader;
        private readonly IEntityRemover<AbstractLabSchedule> _modelRemover;

        public StartpageController(
            IListModelLoader listModelLoader,
            IEntityBasedModelSaver<EditLabScheduleModelBase, AbstractLabSchedule> modelSaver,
            IEntityBasedModelLoader<LabScheduleModel, AbstractLabSchedule> modelLoader,
            IEditLabScheduleModelLoader editModelLoader,
            IEntityRemover<AbstractLabSchedule> modelRemover)
        {
            _listModelLoader = listModelLoader;
            _modelSaver = modelSaver;
            _modelLoader = modelLoader;
            _editModelLoader = editModelLoader;
            _modelRemover = modelRemover;
        }
        
        public ActionResult Index(string message, string ourdatestring = "today")
        {
            var ourdate = DateTime.Today;
            if (!ourdatestring.Equals("today")) ourdate = DateTime.Parse(ourdatestring);
            ViewBag.Message = message;
            var model = _listModelLoader
                 .LoadListModel<LabScheduleListModel, LabScheduleModel>()
                 .FilterByDate(ourdate.AddDays((DayOfWeek.Monday - ourdate.DayOfWeek) * (ourdate.DayOfWeek - ourdate.AddDays(-1).DayOfWeek)),
                    ourdate.AddDays(7 + (DayOfWeek.Monday - ourdate.DayOfWeek) * (ourdate.DayOfWeek - ourdate.AddDays(-1).DayOfWeek)));
            return View(model);
        }

        public ActionResult CreateSchedule(EditLabScheduleModelBase.Kind kind)
        {
            return View(_editModelLoader.CreateEmptyModel(kind));
        }

        [HttpPost]
        public ActionResult CreateSchedule([ModelBinder(typeof(SmartModelBinder))]EditLabScheduleModelBase schedule)
        {
            if (ModelState.IsValid)
            {
                _modelSaver.CreateOrUpdate(schedule);
                ViewBag.Message = "Расписание создано";
                return RedirectToAction("Index");
            }
            ViewBag.Message = "Невозможно сохранить строку расписания";
            return View(schedule);
        }

        public ActionResult EditSchedule(long id = 0)
        {
            return View(_editModelLoader.Load(id));
        }

        [HttpPost]
        public ActionResult EditSchedule([ModelBinder(typeof(SmartModelBinder))]EditLabScheduleModelBase schedule)
        {
            if (ModelState.IsValid)
            {
                _modelSaver.CreateOrUpdate(schedule);
                ViewBag.Message = "Изменения сохранены";
                return RedirectToAction("Index");
            }

            ViewBag.Message = "Невозможно обновить строку расписания";
            return View(schedule);
        }

        public ActionResult Delete(long id = 0)
        {
            return View(_modelRemover.Remove(id));
        }

        [HttpPost]
        public ActionResult Delete([ModelBinder(typeof(SmartModelBinder))]EditLabScheduleModelBase schedule)
        {
            if (ModelState.IsValid)
            {
                _modelSaver.CreateOrUpdate(schedule);
                ViewBag.Message = "Изменения сохранены";
                return RedirectToAction("Index");
            }
            ViewBag.Message = "Невозможно обновить строку расписания";
                  return View(schedule);
         }
    }
}