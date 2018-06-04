using System.Collections.Generic;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Dal.Ef.Repositories
{
    /// <summary> Репозиторий с вопросами </summary>
    internal class SurveyRepository : RepositoryBase, ISurveyRepository
    {
        /// <summary> Репозиторий с вопросами </summary>
        public SurveyRepository(GraphLabsContext context)
            : base(context)
        {
        }

		#region Получение массивов вопросов

		/// <summary> Получить все вопросы </summary>
		public TestQuestion[] GetAllQuestions()
		{
			CheckNotDisposed();
            var result = Context.TestQuestions.ToArray();
            Guard.IsNotNull(result);
            return result; 
		}

		/// <summary> Получить все вопросы в заданной категории </summary>
		public TestQuestion[] GetQuestionByCategory(long categoryId)
		{
            Guard.IsPositive(categoryId, nameof(categoryId));
            CheckNotDisposed();
            var result = Context.TestQuestions.Where(tq => tq.Category.Id == categoryId).ToArray();
            Guard.IsNotNull(result);
            return result;
		}

        public TestQuestion[] GetQuestionsSimilarToString(string criteria)
        {
            CheckNotDisposed();
            var result = Context.TestQuestions.Where(tq => tq.Question.StartsWith(criteria)).ToArray();
            Guard.IsNotNull(result);
            return result;
        }

		#endregion

        ///<summary> Сохранение вопроса </summary>
        public void SaveQuestion(string question, Dictionary<string, bool> questionOptions, long categoryId)
        {
            CheckNotDisposed();

            var quest = Context.TestQuestions.Create();
            quest.Question = question;
            quest.Category = Context.Categories.Single(c => c.Id == categoryId);

            Context.TestQuestions.Add(quest);

            foreach (var answerVar in questionOptions)
            {
                var answerVariant = Context.AnswerVariants.Create();
                answerVariant.TestQuestion = quest;
                answerVariant.IsCorrect = answerVar.Value;
                answerVariant.Answer = answerVar.Key;
				Context.AnswerVariants.Add(answerVariant);
            }

            Context.SaveChanges();
        }

		/// <summary> Получить количество вопросов в категории с id == CategoryId </summary>
		public int GetCategorizesTestQuestionCount(long CategoryId)
		{
            Guard.IsPositive(CategoryId, nameof(CategoryId));
			CheckNotDisposed();

			return Context.TestQuestions.Count(tq => tq.Category.Id == CategoryId);
		}
    }
}
