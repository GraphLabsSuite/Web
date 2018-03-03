using GraphLabs.Site.Controllers.Attributes;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using System;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.Schedule;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
    public class StartpageController : GraphLabsController
    {
        private readonly IListModelLoader _listModelLoader;

        public StartpageController(IListModelLoader listModelLoader)
        {
            _listModelLoader = listModelLoader;
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
    }
}