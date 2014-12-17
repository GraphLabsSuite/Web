using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Controllers.LabWorks;
using GraphLabs.Site.Models;
using GraphLabs.Site.Utils;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web.Routing;

namespace GraphLabs.Site.Controllers
{
	[GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
    public class SurveyController : GraphLabsController
	{
		#region Просмотр списка

		[HttpGet]
		public ActionResult Index(long CategoryId = 0)
		{
			var model = new SurveyIndexViewModel(CategoryId);

			return View("~/Views/Survey/Index.cshtml", model);
		}

		[HttpGet]
		public ActionResult TestQuestionList(long CategoryId)
		{
			var model = new TestQuestionListViewModel(CategoryId);

			return new JsonResult
			{
				Data = RenderHelper.PartialView(this, "~/Views/Survey/TestQuestionListPartial.cshtml", model),
				JsonRequestBehavior = JsonRequestBehavior.AllowGet
			};
		}

		#endregion

		#region Создание вопроса

		[HttpGet]
		public ActionResult Create()
        {
            var emptyQuestion = new SurveyCreatingModel();
            emptyQuestion.Question = "";
            emptyQuestion.QuestionOptions.Add(new KeyValuePair<string, bool>("", true));

            return View("~/Views/Survey/Create.cshtml", emptyQuestion);
        }

        [HttpPost]
        public ActionResult Create(string Question, Dictionary<string, bool> QuestionOptions, long CategoryId)
        {
            var model = new SurveyCreatingModel(Question, QuestionOptions, CategoryId);

            if (model.IsValid)
            {
                model.Save();
				return RedirectToAction("Index", new RouteValueDictionary { { "CategoryId", CategoryId } });
            }

			return View("~/Views/Survey/Create.cshtml", model);
		}

		#endregion
	}
}
