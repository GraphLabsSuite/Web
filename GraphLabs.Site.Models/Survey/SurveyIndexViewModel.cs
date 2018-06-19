using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Models
{
    public class SurveyIndexViewModel
    {
        #region Зависимости

        private readonly ISurveyRepository _surveyRepository;
        private readonly ICategoryRepository _categoryRepository;

        #endregion

        public string SelectedSubCategoryId { get; set; }
        public string SelectedCategoryId { get; set; }

        private List<SelectListItem> _subCategoryList;
        private List<SelectListItem> _categoryList;

        public List<SelectListItem> SubCategoryList
        {
            get
            {
                return _subCategoryList;
            }
        }

        public List<SelectListItem> CategoryList
		{
			get
			{
				return _categoryList;
			}
		}

        public void Load(long SubCategoryId = 0, long CategoryId = 0)
        {
            var catList = _categoryRepository.GetAllCategories();
            var subCatList = _categoryRepository.GetAllSubCategories();
            _categoryList = catList
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
                        Text = "Все темы",
                        Selected = CategoryId == 0
                    }
                })
                .ToList();
            if (CategoryId == 0)
            {
                _subCategoryList = subCatList
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name,
                        Selected = SubCategoryId == c.Id
                    })
                    .Concat(new List<SelectListItem>
                    {
                    new SelectListItem
                    {
                        Value = "0",
                        Text = "Все подтемы",
                        Selected = SubCategoryId == 0
                    }
                    })
                    .ToList();
            }
            else
            {
                _subCategoryList = subCatList
                    .Where(c => c.Category.Id == CategoryId)
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name,
                        Selected = SubCategoryId == c.Id
                    })
                    .Concat(new List<SelectListItem>
                    {
                    new SelectListItem
                    {
                        Value = "0",
                        Text = "Все подтемы",
                        Selected = SubCategoryId == 0
                    }
                    })
                    .ToList();
            }
        }

        public SurveyIndexViewModel(ISurveyRepository surveyRepository, ICategoryRepository categoryRepository)
		{
		    _surveyRepository = surveyRepository;
		    _categoryRepository = categoryRepository;
		}
    }
}