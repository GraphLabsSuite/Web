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
            Guard.IsPositive(id, nameof(id) );
			CheckNotDisposed();
			return Context.Categories.Single(c => c.Id == id);
            var result = Context.Categories.Single(c => c.Id == id);
            Guard.IsNotNull(result);
            return (result);
		}

		///<summary> Получить все категории </summary>
		public Category[] GetAllCategories()
		{
			CheckNotDisposed();
            var result  = Context.Categories.ToArray();
            Guard.IsNotNull(result);
            return result;
		}

        ///<summary> Сохранение категории </summary>
        public void SaveCategory(Category category)
        {
            Guard.IsNotNull(nameof(category), category);
            CheckNotDisposed();

            Context.Categories.Add(category);
			Context.SaveChanges();
        }

		///<summary> Редактирование категории </summary>
		public void EditCategory(Category category)
		{
            Guard.IsNotNull(nameof(category), category);
            CheckNotDisposed();

			Context.Entry(category).State = EntityState.Modified;
			Context.SaveChanges();
		}
    }
}
