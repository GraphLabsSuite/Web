using GraphLabs.DomainModel.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Site.Models
{
    public class CategoryViewModel : BaseViewModel
	{
        #region Зависимости

        private readonly ICategoryRepository _categoriesRepository;

        #endregion

		public long Id { get; set; }

        [Required(ErrorMessage = "Укажите категорию")]
		public string Name { get; set; }

		public CategoryViewModel(ICategoryRepository categoryRepository)
		{
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
				var category = new Category
				{
					Name = this.Name
				};
				_categoriesRepository.SaveCategory(category);
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