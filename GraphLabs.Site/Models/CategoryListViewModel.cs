using GraphLabs.DomainModel;
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

        private ICategoryRepository _categoriesRepository
        {
            get { return DependencyResolver.GetService<ICategoryRepository>(); }
        }

        #endregion

		public long Id { get; set; }

        [Required(ErrorMessage = "Укажите категорию")]
		public string Name { get; set; }

        public void Save()
        {
            var category = new Category
            {
                Name = this.Name
            };
            _categoriesRepository.SaveCategory(category);
        }
	}

	public class CategoryListViewModel
	{
		public CategoryViewModel[] Items { get; private set; }

		public CategoryListViewModel(Category[] categories)
		{
			Items = categories.Select(c => new CategoryViewModel
			{
				Id = c.Id,
				Name = c.Name
			})
			.ToArray();
		}
	}
}