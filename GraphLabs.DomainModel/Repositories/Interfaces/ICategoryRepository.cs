using System;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel.Repositories
{
	/// <summary> Репозиторий с группами </summary>
	[ContractClass(typeof(CategoryRepositoryContracts))]
	public interface ICategoryRepository : IDisposable
	{
		///<summary> Получить все категории </summary>
		Category[] GetAllCategories();
	}

	/// <summary> Репозиторий с группами </summary>
	[ContractClassFor(typeof(ICategoryRepository))]
	internal abstract class CategoryRepositoryContracts : ICategoryRepository
	{
		/// <summary> Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
		public void Dispose()
		{
		}

		public Category[] GetAllCategories()
		{
			Contract.Ensures(Contract.Result<Category[]>() != null);

			return new Category[0];
		}
	}
}
