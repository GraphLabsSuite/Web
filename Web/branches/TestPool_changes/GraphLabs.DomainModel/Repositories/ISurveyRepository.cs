using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.Remoting.Contexts;

namespace GraphLabs.DomainModel.Repositories
{
	/// <summary> Репозиторий с вопросами </summary>
	[ContractClass(typeof(SurveyRepositoryContracts))]
    [Obsolete("Использовать глобальный контекст IGraphLabsContext")]
    public interface ISurveyRepository : IDisposable
	{
		#region Получение массивов вопросов

		/// <summary> Получить все вопросы </summary>
		TestQuestion[] GetAllQuestions();

		/// <summary> Получить все вопросы в заданной категории </summary>
		TestQuestion[] GetQuestionByCategory(long CategoryId);

        /// <summary> Получить все вопросы в заданной подкатегории </summary>
		TestQuestion[] GetQuestionBySubCategory(long subCategoryId);

        /// <summary>
        /// Получить все вопросы, начинающиеся с входного критерия
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
	    TestQuestion[] GetQuestionsSimilarToString(string criteria);

		#endregion

		///<summary> Сохранить вопрос </summary>
		void SaveQuestion(string question, Dictionary<string, bool> questionOptions, long subCategoryId, long categoryId);

		/// <summary> Получить количество вопросов в категории с id == CategoryId </summary>
		int GetCategorizesTestQuestionCount(long CategoryId);
        
	}

    /// <summary> Репозиторий с вопросами </summary>
	[ContractClassFor(typeof(ISurveyRepository))]
    internal abstract class SurveyRepositoryContracts : ISurveyRepository
	{
		/// <summary> Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
		public void Dispose()
		{
		}

		#region Получение массивов вопросов

		public TestQuestion[] GetAllQuestions()
		{
			Contract.Ensures(Contract.Result<TestQuestion[]>() != null);

			return new TestQuestion[0];
		}

		public TestQuestion[] GetQuestionByCategory(long CategoryId)
		{
			Contract.Requires(CategoryId > 0);
			Contract.Ensures(Contract.Result<TestQuestion[]>() != null);

			return new TestQuestion[0];
		}

        public TestQuestion[] GetQuestionBySubCategory(long subCategoryId)
        {
            Contract.Requires(subCategoryId > 0);
            Contract.Ensures(Contract.Result<TestQuestion[]>() != null);

            return new TestQuestion[0];
        }

        public TestQuestion[] GetQuestionsSimilarToString(string criteria)
        {
            Contract.Ensures(Contract.Result<TestQuestion[]>() != null);
            return new TestQuestion[0];
        }

        #endregion

        public void SaveQuestion(string question, Dictionary<string, bool> questionOptions, long subCategoryId, long categoryId)
        {

        }

		public int GetCategorizesTestQuestionCount(long CategoryId)
		{
			Contract.Requires(CategoryId > 0);

			return default(int);
		}
	}
}
