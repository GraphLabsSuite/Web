using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.DomainModel.Services;
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
		#region Зависимости

		private ICategoryRepository _categoriesRepository
		{
			get { return DependencyResolver.GetService<ICategoryRepository>(); }
		}

		#endregion
		
		public ActionResult Index()
        {
			var model = new CategoryListViewModel(_categoriesRepository.GetAllCategories());

            return View("~/Views/Category/Index.cshtml", model);
		}

		public ActionResult Create()
		{
			throw new NotImplementedException();
		}
	}
}
