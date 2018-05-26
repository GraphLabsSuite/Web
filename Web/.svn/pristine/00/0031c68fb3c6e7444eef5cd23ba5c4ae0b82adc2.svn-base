using GraphLabs.Dal.Ef;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Models;
using System.Linq;
using System.Web.Mvc;
using System;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.Site.Models.Question;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Controllers
{
	[GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
	public class CategoryController : GraphLabsController
	{
	    private readonly ICategoryRepository _categoryRepository;
	    private readonly ITestsContext _testsContext;
        private readonly IEntityBasedModelSaver<CategoryModel, Category> _categorySaver;


        public CategoryController(ICategoryRepository categoryRepository, ITestsContext testsContext,
            IEntityBasedModelSaver<CategoryModel, Category> categorySaver)
	    {
	        _categoryRepository = categoryRepository;
	        _testsContext = testsContext;
            _categorySaver = categorySaver;
	    }

	    #region Просмотр списка

		public ActionResult Index()
        {
			var model = new CategoryListViewModel();

            return View("~/Views/Category/Index.cshtml", model);
		}

		#endregion

		#region Создание и редактирование

		[HttpGet]
		public ActionResult Create()
		{
            var model = new CategoryViewModel(_testsContext, _categoryRepository);

            return View("~/Views/Category/Create.cshtml", model);
		}

		[HttpGet]
		public ActionResult Edit(long Id)
		{
            var model = new CategoryViewModel(_testsContext, _categoryRepository);
            model.Load(Id);

			return View("~/Views/Category/Create.cshtml", model);
		}

        [HttpPost]
        public ActionResult Create(CategoryViewModel request)
        {
            if (ModelState.IsValid)
            {
                request.Save();
                return RedirectToAction("Index");
            }

            return View("~/Views/Category/Create.cshtml", request);
		}

        /*[HttpGet]
        public ActionResult AddCategory()
        {
            var model= new SurveyCreatingModel(_surveyRepository, _categoryRepository);
            return View("~/Views/Survey/AddCategory.cshtml", model);
        }*/

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
        #endregion
    }
}
