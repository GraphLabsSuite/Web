using GraphLabs.Dal.Ef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Site.Models
{
    public class CategoryViewModel : BaseViewModel
	{
        #region Зависимости

        private readonly ITestsContext _testsContext;
        private readonly ICategoryRepository _categoriesRepository;

        #endregion

		public long Id { get; set; }

        [Required(ErrorMessage = "Укажите категорию")]
		public string Name { get; set; }

		public CategoryViewModel(ITestsContext testsContext, ICategoryRepository categoryRepository)
		{
		    _testsContext = testsContext;
		    _categoriesRepository = categoryRepository;
		}

        public void Load(long id)
        {
            var category = _categoriesRepository.GetById(id);
            Id = category.Id;
            Name = category.Name;
        }

        public void Save()
        {
			if (Id == default(int))
			{
			    var category = _testsContext.Categories.CreateNew();
			    category.Name = this.Name;
			}
			else
			{
				var category = _categoriesRepository.GetById(Id);
				category.Name = Name;
				_categoriesRepository.EditCategory(category);
			}
        }
	}
}