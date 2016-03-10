using GraphLabs.DomainModel.EF;
using GraphLabs.DomainModel.EF.Services;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Models;
using System.Linq;
using System.Web.Mvc;
using System;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Site.Controllers
{
	[GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
	public class CategoryController : GraphLabsController
	{
	    private readonly ICategoryRepository _categoryRepository;
	    private readonly ITestsContext _testsContext;

	    public CategoryController(ICategoryRepository categoryRepository, ITestsContext testsContext)
	    {
	        _categoryRepository = categoryRepository;
	        _testsContext = testsContext;
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

		#endregion
	}
}
