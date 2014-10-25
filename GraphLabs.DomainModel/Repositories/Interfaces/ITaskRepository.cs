using JetBrains.Annotations;

namespace GraphLabs.DomainModel.Repositories
{
    /// <summary> Репозиторий заданий </summary>
    public interface ITaskRepository
    {
        /// <summary> Получить все задания </summary>
        [NotNull]
        Task[] GetAllTasks();

        /// <summary> Найти по ID </summary>
        [CanBeNull]
        Task FindById(long id);

		/// <summary> Получить вариант задания по id </summary>
		[NotNull]
		TaskVariant GetTaskVariantById(long id);

        /// <summary> Создать или обновить вариант </summary>
        [NotNull]
        TaskVariant CreateOrUpdateVariant([NotNull]TaskVariant variant);

        /// <summary> Есть уже задание с таким же именем и версией? </summary>
        bool IsAnySameTask(string name, string version);

        /// <summary> Добавить в базу </summary>
        void Insert(Task task);
    }
}