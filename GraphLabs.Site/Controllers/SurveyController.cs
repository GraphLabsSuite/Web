using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Models;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.Question;
using GraphLabs.Site.Models.TestPool;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
    public class SurveyController : GraphLabsController
	{
	    private readonly ISurveyRepository _surveyRepository;
	    private readonly ICategoryRepository _categoryRepository;
        private readonly IEntityBasedModelLoader<TestPoolModel, TestPool> _modelLoader;
        private readonly IEntityBasedModelSaver<CategoryModel, Category> _categorySaver;

        public SurveyController(
            ISurveyRepository surveyRepository,
            ICategoryRepository categoryRepository,
            IEntityBasedModelLoader<TestPoolModel, TestPool> modelLoader,
            IEntityBasedModelSaver<CategoryModel, Category> categorySaver)
        {
            _modelLoader = modelLoader;
	        _surveyRepository = surveyRepository;
	        _categoryRepository = categoryRepository;
            _categorySaver = categorySaver;
	    }

	    #region Просмотр списка

		[HttpGet]
		public ActionResult Index(long SubCategoryId = 0, long CategoryId = 0)
		{
            var model = new SurveyIndexViewModel(_surveyRepository, _categoryRepository);
            model.Load(SubCategoryId, CategoryId);

			return View("~/Views/Survey/Index.cshtml", model);
		}

		[HttpGet]
		public ActionResult TestQuestionList(long SubCategoryId = 0, long CategoryId = 0)
		{
			var model = new TestQuestionListViewModel(_surveyRepository);
            model.Load(SubCategoryId, CategoryId);

			return new JsonResult
			{
				Data = RenderHelper.PartialView(this, "~/Views/Survey/TestQuestionListPartial.cshtml", model),
				JsonRequestBehavior = JsonRequestBehavior.AllowGet
			};
		}

	    [HttpPost]
	    public ActionResult Load(QuestionLookForModel input)
	    {
	        var questions = _surveyRepository.GetQuestionsSimilarToString(input.Question);
            var questionArray = questions.Select(q => new Tuple<string, long>(
            q.Question,q.Id))
                .ToArray();
            var json = Json(questionArray);
	        return json;
	    }

	    [HttpPost]
	    public ActionResult LoadUnique(QuestionLookForModel input)
	    {
            //// Новый код подгружает только те вопросы, которых ещё нет в данном тестпуле
            //var entity = _modelLoader.Load(input.TestPool);
            //var questions = _surveyRepository.GetQuestionsSimilarToString(input.Question);
            //var questionArray = questions
            //       .Where(q => entity.TestPoolEntries.All(t => t.TestQuestion.Question != q.Question))
            //       .Select(q => new Tuple<string, long>(q.Question, q.Id))
            //       .ToArray();
            //   var json = Json(questionArray);
            //   return json;
            return null;
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

        [HttpGet]
        public ActionResult Edit(string Question)
        {
            var model = new SurveyCreatingModel(_surveyRepository, _categoryRepository)
            {
                Question = Question
            };
            return View("~/Views/Survey/Edit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(string Question, Dictionary<string, bool> QuestionOptions, long CategoryId)
        {
            //Question, QuestionOptions, CategoryId
            var model = new SurveyCreatingModel(_surveyRepository, _categoryRepository)
            {
                CategoryId = CategoryId,
                QuestionOptions = QuestionOptions.ToList().Where(x => x.Key != "controller" && x.Key != "action").ToList(),
                Question = Question
            };

            if (model.IsValid)
            {
                model.Save();
                return RedirectToAction("Index", new RouteValueDictionary { { "CategoryId", CategoryId } }); //Надо настроить редирект
            }

            return View("~/Views/Survey/Edit.cshtml", model); //Надо настроить редирект
        }

        /*[HttpGet]
        public ActionResult AddCategory()
        {
            var model= new SurveyCreatingModel(_surveyRepository, _categoryRepository);
            return View("~/Views/Survey/AddCategory.cshtml", model);
        }*/

       /* [HttpPost]
        public ActionResult AddCategory(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                _categorySaver.CreateOrUpdate(category);
                return RedirectToAction("Index");
            }

            ViewBag.Message = "Невозможно сохранить категорию";
            return View(category);
        }*/
        /*     public ActionResult Create(GroupModel group)
        {
            if (ModelState.IsValid)
            {
                _modelSaver.CreateOrUpdate(group);
                return RedirectToAction("Index");
            }

            ViewBag.Message = "Невозможно сохранить группу";
            return View(group);
        }*/





        [HttpGet]
        public ActionResult AddSubCategory()
        {
            var model = new SurveyCreatingModel(_surveyRepository, _categoryRepository);
            return View("~/Views/Survey/AddSubCategory.cshtml", model);
        }

        [HttpPost]
        public ActionResult AddSubCategory(string Question, Dictionary<string, bool> QuestionOptions, long CategoryId)
        {
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

            return View("~/Views/Survey/AddSubCategory.cshtml", model);
        }
        #endregion
    }
}
