using System.Collections.Generic;
using System.Linq;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Site.Models
{
    public class TestQuestionListViewModel
    {
        #region Зависимости

        private readonly ISurveyRepository _surveyRepository;

        #endregion

		public bool ShowCategory { get; set; }

		public List<TestQuestionDto> Items { get; private set; }

        public void Load(long SubCategoryId)
        {
            ShowCategory = SubCategoryId == 0;

            var questions = SubCategoryId == 0
                ? _surveyRepository.GetAllQuestions()
                : _surveyRepository.GetQuestionByCategory(SubCategoryId);

            Items = questions.Select(q => new TestQuestionDto
            {
                QuestionId = q.Id,
                Question = q.Question,
                QuestionSubCategory = q.SubCategory.Name,
            })
                .ToList();
        }
        public TestQuestionListViewModel(ISurveyRepository surveyRepository)
		{
		    _surveyRepository = surveyRepository;
		}
    }

	public class TestQuestionDto
	{
		public long QuestionId { get; set; }
		public string Question { get; set; }
		public string QuestionSubCategory { get; set; }
    }
}