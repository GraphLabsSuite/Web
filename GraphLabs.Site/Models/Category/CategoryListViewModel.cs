using System;
using System.Linq;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Site.Models
{
	public class CategoryListViewModel : BaseViewModel
	{
        #region Зависимости

        private readonly ICategoryRepository _categoriesRepository;
	    private readonly ISurveyRepository _surveyRepository;

		#endregion

        public CategoryListViewModel(ICategoryRepository categoriesRepository, ISurveyRepository surveyRepository)
        {
            _categoriesRepository = categoriesRepository;
            _surveyRepository = surveyRepository;
        }

	    public CategoryViewModelDto[] Items { get; private set; }

		public CategoryListViewModel()
		{
			Items = _categoriesRepository.GetAllCategories()
			.Select(c => new CategoryViewModelDto(_categoriesRepository)
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
	    public CategoryViewModelDto(ICategoryRepository categoryRepository) : base(categoryRepository)
	    {
	    }

	    public int QuestionCount { get; set; }
	}
}