using System;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace GraphLabs.DomainModel.Repositories
{
	/// <summary> Репозиторий с вопросами </summary>
	[ContractClass(typeof(SurveyRepositoryContracts))]
    public interface ISurveyRepository : IDisposable
	{
		///<summary> Сохранить вопрос </summary>
		void SaveQuestion(string question, Dictionary<string, bool> questionOptions, long categoryId);
        
	}

    /// <summary> Репозиторий с вопросами </summary>
	[ContractClassFor(typeof(ISurveyRepository))]
    internal abstract class SurveyRepositoryContracts : ISurveyRepository
	{
		/// <summary> Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
		public void Dispose()
		{
		}

        public void SaveQuestion(string question, Dictionary<string, bool> questionOptions, long categoryId)
        {

        }
	}
}
