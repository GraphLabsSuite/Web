using System.Linq;

namespace GraphLabs.DomainModel.Repositories
{
    /// <summary> Репозиторий заданий </summary>
    internal class TaskRepository : RepositoryBase, ITaskRepository
    {
        /// <summary> Репозиторий заданий </summary>
        public TaskRepository(GraphLabsContext context) : base(context)
        {
        }

        /// <summary> Получить все задания </summary>
        public Task[] GetAllTasks()
        {
            return Context
                .Tasks
                .OrderBy(t => t.Id)
                .ToArray();
        }

        /// <summary> Найти по ID </summary>
        public Task FindById(long id)
        {
            return Context.Tasks.Find(id);
        }

		/// <summary> Получить вариант задания по id </summary>
		public TaskVariant GetTaskVariantById(long id)
		{
			return Context.TaskVariants.Single(tv => tv.Id == id);
		}

        /// <summary> Есть уже задание с таким же именем и версией? </summary>
        public bool IsAnySameTask(string name, string version)
        {
            return Context.Tasks.Any(t => t.Name == name && t.Version == version);
        }

        /// <summary> Сохранить </summary>
        public void Insert(Task task)
        {
            Context.Tasks.Add(task);
        }
    }
}
