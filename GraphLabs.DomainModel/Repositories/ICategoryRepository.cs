using System;
using System.Diagnostics.Contracts;

namespace GraphLabs.DomainModel.Repositories
{
	/// <summary> Репозиторий с группами </summary>
	[ContractClass(typeof(CategoryRepositoryContracts))]
	public interface ICategoryRepository : IDisposable
	{
		/// <summary> Получить категорию по id </summary>
		Category GetById(long id);

		///<summary> Получить все категории </summary>
		Category[] GetAllCategories();

        ///<summary> Сохранение категории </summary>
        void SaveCategory(Category category);

		///<summary> Редактирование категории </summary>
		void EditCategory(Category category);
	}

	/// <summary> Репозиторий с группами </summary>
	[ContractClassFor(typeof(ICategoryRepository))]
	internal abstract class CategoryRepositoryContracts : ICategoryRepository
	{
		/// <summary> Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
		public void Dispose()
		{
		}

		public Category GetById(long id)
		{
			Contract.Requires(id > 0);
			Contract.Ensures(Contract.Result<Category>() != null);

			return default(Category);
		}

		public Category[] GetAllCategories()
		{
			Contract.Ensures(Contract.Result<Category[]>() != null);

			return new Category[0];
		}

        public void SaveCategory(Category category)
        {
            Contract.Requires(category != null);
        }

		public void EditCategory(Category category)
		{
			Contract.Requires(category != null);
		}
	}
}
