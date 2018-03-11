using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Site.Models
{
    public class SurveyIndexViewModel
    {
        #region Зависимости

        private readonly ISurveyRepository _surveyRepository;
        private readonly ICategoryRepository _categoryRepository;

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

        public void Load(long CategoryId = 0)
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
                .ToList()
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

		public SurveyIndexViewModel(ISurveyRepository surveyRepository, ICategoryRepository categoryRepository)
		{
		    _surveyRepository = surveyRepository;
		    _categoryRepository = categoryRepository;
		}
    }
}