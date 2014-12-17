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
    public class SurveyIndexViewModel : BaseViewModel
    {
        #region Зависимости

        private ISurveyRepository _surveyRepository
        {
            get { return DependencyResolver.GetService<ISurveyRepository>(); }
        }       

        private ICategoryRepository _categoryRepository
        {
            get { return DependencyResolver.GetService<ICategoryRepository>(); }
        }

        #endregion

		public string SelectedCategoryId { get; set; }

		private List<SelectListItem> _categoryList;

		public List<SelectListItem> CategoryList
		{
			get
			{
				return _categoryList;
			}
		}

		public SurveyIndexViewModel(long CategoryId = 0)
		{
			_categoryList = _categoryRepository.GetAllCategories()
					.Select(c => new SelectListItem
					{
						Value = c.Id.ToString(),
						Text = c.Name,
						Selected = CategoryId == c.Id
					})
					.Concat(new List<SelectListItem>
					{
						new SelectListItem
						{
							Value = "0",
							Text = "Все категории",
							Selected = CategoryId == 0
						}
					})
					.ToList();
		}
    }
}