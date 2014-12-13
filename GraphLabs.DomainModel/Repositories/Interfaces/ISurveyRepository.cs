using System;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel.Repositories
{
	/// <summary> Репозиторий с группами </summary>
	[ContractClass(typeof(CategoryRepositoryContracts))]
    public interface ISurveyRepository : IDisposable
	{
		///<summary> Получить все категории </summary>
		Category[] GetAllCategories();



        //тут методы



	}

	/// <summary> Репозиторий с группами </summary>
	[ContractClassFor(typeof(ICategoryRepository))]
    internal abstract class SurveyRepositoryContracts : ICategoryRepository
	{
		/// <summary> Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
		public void Dispose()
		{
		}

	}
}
