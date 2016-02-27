using GraphLabs.DomainModel.EF;
using GraphLabs.DomainModel.EF.Repositories;
using GraphLabs.DomainModel.EF.Services;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Models;
using System.Linq;
using System.Web.Mvc;
using System;

namespace GraphLabs.Site.Controllers
{
	[GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
	public class CategoryController : GraphLabsController
	{
	    private readonly ICategoryRepository _categoryRepository;

	    public CategoryController(ICategoryRepository categoryRepository)
	    {
	        _categoryRepository = categoryRepository;
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
            var model = new CategoryViewModel(_categoryRepository);

            return View("~/Views/Category/Create.cshtml", model);
		}

		[HttpGet]
		public ActionResult Edit(long Id)
		{
            var model = new CategoryViewModel(_categoryRepository);
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
