using System.Linq;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.DomainModel.EF.Repositories
{
    /// <summary> Репозиторий с группами </summary>
    internal class SurveyRepository : RepositoryBase, ISurveyRepository
    {
        /// <summary> Репозиторий с группами </summary>
        public SurveyRepository(GraphLabsContext context)
            : base(context)
        {
        }

		#region Получение массивов вопросов

		/// <summary> Получить все вопросы </summary>
		public TestQuestion[] GetAllQuestions()
		{
			CheckNotDisposed();

			return Context.TestQuestions.ToArray();
		}

		/// <summary> Получить все вопросы в заданной категории </summary>
		public TestQuestion[] GetQuestionByCategory(long CategoryId)
		{
			CheckNotDisposed();

			return Context.TestQuestions.Where(tq => tq.Category.Id == CategoryId).ToArray();
		}

		#endregion

        ///<summary> Сохранение вопроса </summary>
        public void SaveQuestion(string question, Dictionary<string, bool> questionOptions, long categoryId)
        {
            CheckNotDisposed();
                        
            var quest = new TestQuestion
            {
                Question = question,
                Category = Context.Categories.Single(c => c.Id == categoryId)
            };

            Context.TestQuestions.Add(quest);

            foreach (var answerVar in questionOptions)
            {
                var answerVariant = new AnswerVariant
                {
                    TestQuestion = quest,
                    IsCorrect = answerVar.Value,
                    Answer = answerVar.Key
                };
				Context.AnswerVariants.Add(answerVariant);
            }

            Context.SaveChanges();
        }

		/// <summary> Получить количество вопросов в категории с id == CategoryId </summary>
		public int GetCategorizesTestQuestionCount(long CategoryId)
		{
			CheckNotDisposed();

			return Context.TestQuestions.Count(tq => tq.Category.Id == CategoryId);
		}
    }
}
