using System.Linq;
using System.Data.Entity;
using System;

namespace GraphLabs.DomainModel.Repositories
{
    /// <summary> Репозиторий с группами </summary>
    internal class CategoryRepository : RepositoryBase, ICategoryRepository
    {
        /// <summary> Репозиторий с группами </summary>
        public CategoryRepository(GraphLabsContext context)
            : base(context)
        {
        }

		///<summary> Получить все категории </summary>
		public Category[] GetAllCategories()
		{
			CheckNotDisposed();

			return Context.Categories.ToArray();
		}
    }
}
