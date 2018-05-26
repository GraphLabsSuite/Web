using System;
using System.Linq;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Site.Models
{
	public class CategoryListViewModel : BaseViewModel
	{
        #region Зависимости

	    private readonly ITestsContext _testsContext;
	    private readonly ICategoryRepository _categoriesRepository;
	    private readonly ISurveyRepository _surveyRepository;

		#endregion

        public CategoryListViewModel(ITestsContext testsContext, ICategoryRepository categoriesRepository, ISurveyRepository surveyRepository)
        {
            _testsContext = testsContext;
            _categoriesRepository = categoriesRepository;
            _surveyRepository = surveyRepository;
        }

	    public CategoryViewModelDto[] Items { get; private set; }

		public CategoryListViewModel()
		{
            //TODO: null exception при отображении категорий
		    if (Items != null)
		    {
		        Items = _categoriesRepository.GetAllCategories()
		            .Select(c => new CategoryViewModelDto(_testsContext, _categoriesRepository)
		            {
		                Id = c.Id,
		                Name = c.Name,
		                QuestionCount = _surveyRepository.GetCategorizesTestQuestionCount(c.Id)
		            })
		            .ToArray();
		    }
		    else
		    {
		        Items = new CategoryViewModelDto[0];
		    }
		}
	}

	public class CategoryViewModelDto : CategoryViewModel
	{
	    public CategoryViewModelDto(ITestsContext testsContext, ICategoryRepository categoryRepository) : base(testsContext, categoryRepository)
	    {
	    }

	    public int QuestionCount { get; set; }
	}
}