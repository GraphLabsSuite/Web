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
using System.Web.Helpers;
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
        public ActionResult Index(long SubCategoryId = 1, long CategoryId = 1)
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
            q.Question, q.Id))
                .ToArray();
            var json = Json(questionArray);
            return json;
        }

        [HttpPost]
        public ActionResult LoadUnique(QuestionLookForModel input)
        {
            // Новый код подгружает только те вопросы, которых ещё нет в данном тестпуле
            var entity = _modelLoader.Load(input.TestPool);
            var questions = _surveyRepository.GetQuestionsSimilarToString(input.Question);
            var questionArray = questions
                .Where(q => entity.TestPoolEntries.All(t => t.TestQuestion.Question != q.Question))
                .Select(q => new Tuple<string, long>(q.Question, q.Id))
                .ToArray();
            var json = Json(questionArray);
            return json;
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
        public ActionResult Create(string Question, Dictionary<string, bool> QuestionOptions, long CategoryId, long SubCategoryId)
        {
            long QuestionId = _surveyRepository.GetAllQuestions().Select(q => q.Id).Max() + 1;
            var model = new SurveyCreatingModel(_surveyRepository, _categoryRepository)
            {
                QuestionId = QuestionId,
                CategoryId = CategoryId,
                SubCategoryId = SubCategoryId,
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

        #region Редактирование вопроса

        [HttpGet]
        public ActionResult Edit(long QuestionId)
        {
            var questions = _surveyRepository.GetAllQuestions();
            var question = questions.Where(q => q.Id == QuestionId).First();

            var model = new SurveyCreatingModel(_surveyRepository, _categoryRepository)
            {
                QuestionId = question.Id,
                Question = question.Question,
                CategoryId = question.SubCategory.Category.Id,
                SubCategoryId = question.SubCategory.Id,
                QuestionOptions = question.AnswerVariants.Select(e => new KeyValuePair<String, Boolean>(e.Answer, e.IsCorrect)).ToList()
            };
            return View("~/Views/Survey/Edit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(string Question, Dictionary<string, bool> QuestionOptions, long CategoryId, long SubCategoryId, long QuestionId)
        {
            var model = new SurveyCreatingModel(_surveyRepository, _categoryRepository)
            {
                QuestionId = QuestionId,
                CategoryId = CategoryId,
                SubCategoryId = SubCategoryId,
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

        #endregion

        #region Удаление вопроса

        [HttpGet]
        public ActionResult ViewQuestion(long QuestionId, String questionText)
        {
            TestQuestion question = _surveyRepository.GetAllQuestions().Where(q => q.Id == QuestionId).First();
            var model = new SurveyCreatingModel(_surveyRepository, _categoryRepository)
            {
                QuestionId = QuestionId,
                Question = questionText,
                CategoryId = question.SubCategory.Category.Id,
                SubCategoryId = question.SubCategory.Id,
                QuestionOptions = question.AnswerVariants.Select(e => new KeyValuePair<String, Boolean>(e.Answer, e.IsCorrect)).ToList()
            };
            return View("~/Views/Survey/ViewQuestion.cshtml", model);
        }

        [HttpPost]
        public ActionResult ViewQuestion(long questionId)
        {
            TestQuestion question = _surveyRepository.GetAllQuestions().Where(q => q.Id == questionId).First();
            var model = new SurveyCreatingModel(_surveyRepository, _categoryRepository)
            {
                QuestionId = question.Id,
                CategoryId = question.SubCategory.Category.Id,
                SubCategoryId = question.SubCategory.Id,
                QuestionOptions = question.AnswerVariants.Select(e => new KeyValuePair<String, Boolean>(e.Answer, e.IsCorrect)).ToList(),
                Question = question.Question
            };

            model.Delete();

            //return View("~/Views/Survey/Index.cshtml"); //Надо настроить редирект   
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult AddCategory()
        {

            return View("~/Views/Survey/AddCategory.cshtml");
        }

        [HttpPost]
        public ActionResult AddCategory(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                _categorySaver.CreateOrUpdate(category);
                return RedirectToAction("Index");
            }

            ViewBag.Message = "Невозможно сохранить категорию";
            return View(category);
        }

        [HttpGet]
        public ActionResult AddSubCategory()
        {
            var emptySubCategory = new SubCategoryCreatingModel(_surveyRepository, _categoryRepository);
            emptySubCategory.Name = "";
            emptySubCategory.CategoryId = 1;
            return View("~/Views/Survey/AddSubCategory.cshtml", emptySubCategory);
        }

        [HttpPost]
        public ActionResult AddSubCategory(long categoryId, String name)
        {
            long subCategoryId = _categoryRepository.GetAllSubCategories().Select(q => q.Id).Max() + 1;
            var model = new SubCategoryCreatingModel(_surveyRepository, _categoryRepository)
            {
                SubCategoryId = subCategoryId,
                CategoryId = categoryId,
                Name = name
            };

            if (model.IsValid)
            {
                model.Save();
                return RedirectToAction("Index", new RouteValueDictionary { { "CategoryId", categoryId } });
            }

            return View("~/Views/Survey/AddSubCategory.cshtml", model);


        }
        #endregion
    }
}
