using System;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel.Repositories
{
	/// <summary> Репозиторий с группами </summary>
	[ContractClass(typeof(SurveyRepositoryContracts))]
    public interface ISurveyRepository : IDisposable
	{
		///<summary> Получить все категории </summary>
		//Category[] GetAllCategories();



        //тут методы



	}

	/// <summary> Репозиторий с группами </summary>
	[ContractClassFor(typeof(ISurveyRepository))]
    internal abstract class SurveyRepositoryContracts : ISurveyRepository
	{
		/// <summary> Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
		public void Dispose()
		{
		}

	}
}
