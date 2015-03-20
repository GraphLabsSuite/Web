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
    public class TestQuestionListViewModel : BaseViewModel
    {
        #region Зависимости

        private readonly ISurveyRepository _surveyRepository;

        #endregion

		public bool ShowCategory { get; set; }

		public List<TestQuestionDto> Items { get; private set; }

        public void Load(long CategoryId)
        {
            ShowCategory = CategoryId == 0;

            var questions = CategoryId == 0
                ? _surveyRepository.GetAllQuestions()
                : _surveyRepository.GetQuestionByCategory(CategoryId);

            Items = questions.Select(q => new TestQuestionDto
            {
                QuestionId = q.Id,
                Question = q.Question,
                QuestionCategory = q.Category.Name
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
		public string QuestionCategory { get; set; }
	}
}