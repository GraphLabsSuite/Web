using GraphLabs.Dal.Ef;
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
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Site.Controllers
{
	[GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
    public class SurveyController : GraphLabsController
	{
	    private readonly ISurveyRepository _surveyRepository;
	    private readonly ICategoryRepository _categoryRepository;

	    public SurveyController(ISurveyRepository surveyRepository, ICategoryRepository categoryRepository)
	    {
	        _surveyRepository = surveyRepository;
	        _categoryRepository = categoryRepository;
	    }

	    #region Просмотр списка

		[HttpGet]
		public ActionResult Index(long CategoryId = 0)
		{
            var model = new SurveyIndexViewModel(_surveyRepository, _categoryRepository);
            model.Load(CategoryId);

			return View("~/Views/Survey/Index.cshtml", model);
		}

		[HttpGet]
		public ActionResult TestQuestionList(long CategoryId)
		{
			var model = new TestQuestionListViewModel(_surveyRepository);
            model.Load(CategoryId);

			return new JsonResult
			{
				Data = RenderHelper.PartialView(this, "~/Views/Survey/TestQuestionListPartial.cshtml", model),
				JsonRequestBehavior = JsonRequestBehavior.AllowGet
			};
		}

	    [HttpGet]
	    public Tuple<string, long>[] Load(string input)
	    {
	        TestQuestion[] questions = _surveyRepository.GetQuestionsSimilarToString(input);
            return questions.Select(q => new Tuple<string, long>(
            q.Question,q.Id))
                .ToArray();
        }

		#endregion

		#region Создание вопроса

		[HttpGet]
		public ActionResult Create()
        {
            var emptyQuestion = new SurveyCreatingModel(_surveyRepository, _categoryRepository);
            emptyQuestion.Question = "";
            emptyQuestion.QuestionOptions.Add(new KeyValuePair<string, bool>("", true));

            return View("~/Views/Survey/Create.cshtml", emptyQuestion);
        }

        [HttpPost]
        public ActionResult Create(string Question, Dictionary<string, bool> QuestionOptions, long CategoryId)
        {
            //Question, QuestionOptions, CategoryId
            var model = new SurveyCreatingModel(_surveyRepository, _categoryRepository)
            {
                CategoryId = CategoryId,
                QuestionOptions = QuestionOptions.ToList(),
                Question = Question
            };

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
