using System;
using System.Collections.Generic;
using System.Linq;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Models
{
    public class TestQuestionListViewModel
    {
        #region Зависимости

        private readonly ISurveyRepository _surveyRepository;

        #endregion

		public bool ShowSubCategory { get; set; }
        public bool ShowCategory { get; set; }

		public List<TestQuestionDto> Items { get; private set; }

        public void Load(long SubCategoryId, long CategoryId)
        {
            ShowSubCategory = SubCategoryId != 0;
            ShowCategory = CategoryId != 0;

            var questions = _surveyRepository.GetAllQuestions();

            if (ShowCategory) {
                if (ShowSubCategory)
                {
                    questions = _surveyRepository.GetQuestionByCategory(CategoryId);
                    //questions = _surveyRepository.GetQuestionsBySubCategory(SubCategoryId);
                }
                else
                {
                    questions = _surveyRepository.GetQuestionByCategory(CategoryId);
                }
            }

            /*var questions = SubCategoryId == 0
                ? _surveyRepository.GetAllQuestions()
                : _surveyRepository.GetQuestionByCategory(SubCategoryId);*/

            //var questions = CategoryId == 
            
            Items = questions.Select(q => new TestQuestionDto
            {
                QuestionId = q.Id,
                Question = q.Question,
                QuestionSubCategory = q.SubCategory.Name,
                QuestionCategory = q.SubCategory.Category.Name,
                QuestionSubCategoryId = q.SubCategory.Id,
                QuestionCategoryId = q.SubCategory.Category.Id,
                StringAnswers = q.AnswerVariants.Select(a => a.Answer).ToList(),
                RightAnswers = q.AnswerVariants.Select(a => a.IsCorrect).ToList(),
                Answers = q.AnswerVariants.Select(e => new KeyValuePair<String, Boolean>(e.Answer, e.IsCorrect)).ToList()

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
        public long QuestionCategoryId { get; set; }
        public string QuestionCategory { get; set; }
        public long QuestionSubCategoryId { get; set; }
        public string QuestionSubCategory { get; set; }
        public IList<String> StringAnswers { get; set; }
        public IList<bool> RightAnswers { get; set; }
        public List<KeyValuePair<String, bool>> Answers { get; set; }
    }
}