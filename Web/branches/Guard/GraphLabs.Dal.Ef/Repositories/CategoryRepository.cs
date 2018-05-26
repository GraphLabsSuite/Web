using System.Data.Entity;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Dal.Ef.Repositories
{
    /// <summary> Репозиторий с группами </summary>
    internal class CategoryRepository : RepositoryBase, ICategoryRepository
    {
        /// <summary> Репозиторий с группами </summary>
        public CategoryRepository(GraphLabsContext context)
            : base(context)
        {
        }

		/// <summary> Получить категорию по id </summary>
		public Category GetById(long id)
		{
			CheckNotDisposed();

			return Context.Categories.Single(c => c.Id == id);
		}

		///<summary> Получить все категории </summary>
		public Category[] GetAllCategories()
		{
			CheckNotDisposed();

			return Context.Categories.ToArray();
		}

        ///<summary> Сохранение категории </summary>
        public void SaveCategory(Category category)
        {
            CheckNotDisposed();

            Context.Categories.Add(category);
			Context.SaveChanges();
        }

		///<summary> Редактирование категории </summary>
		public void EditCategory(Category category)
		{
			CheckNotDisposed();

			Context.Entry(category).State = EntityState.Modified;
			Context.SaveChanges();
		}
    }
}
