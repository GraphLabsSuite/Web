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
	public class CategoryListViewModel : BaseViewModel
	{
		#region Зависимости

		private ICategoryRepository _categoriesRepository
		{
			get { return DependencyResolver.GetService<ICategoryRepository>(); }
		}

		private ISurveyRepository _surveyRepository
		{
			get { return DependencyResolver.GetService<ISurveyRepository>(); }
		}

		#endregion

		public CategoryViewModelDto[] Items { get; private set; }

		public CategoryListViewModel()
		{
			Items = _categoriesRepository.GetAllCategories()
			.Select(c => new CategoryViewModelDto
			{
				Id = c.Id,
				Name = c.Name,
				QuestionCount = _surveyRepository.GetCategorizesTestQuestionCount(c.Id)
			})
			.ToArray();
		}
	}

	public class CategoryViewModelDto : CategoryViewModel
	{
		public int QuestionCount { get; set; }
	}
}