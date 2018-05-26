﻿using System;
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
                    questions = _surveyRepository.GetQuestionBySubCategory(SubCategoryId);
                }
                else
                {
                    questions = _surveyRepository.GetQuestionByCategory(CategoryId);
                }
            }
                        
            Items = questions.Select(q => new TestQuestionDto
            {
                QuestionId = q.Id,
                Question = q.Question,
                Score = q.Score,
                ScoringStrategy = q.ScoringStrategy.ToString(),
                QuestionSubCategory = q.SubCategory.Name,
                QuestionCategory = q.SubCategory.Category.Name,                
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
        public int Score { get; set; }
        public string ScoringStrategy { get; set; }
        public string QuestionCategory { get; set; }
        public string QuestionSubCategory { get; set; }
    }
}