using System.IO;
using GraphLabs.DomainModel.EF;
using JetBrains.Annotations;

namespace GraphLabs.Site.Logic.Tasks
{
    /// <summary> Менеджер заданий </summary>
    public interface ITaskManager
    {
        /// <summary> Загрузить задание </summary>
        [CanBeNull]
        Task UploadTask([NotNull]Stream stream);

        /// <summary> Загрузить задание и прописать в название время загрузки </summary>
        /// <remarks> Для отладочных целей </remarks>
        [CanBeNull]
        Task UploadTaskWithTimestamp(Stream stream);

        /// <summary> Установить заданию генератор </summary>
        void SetGenerator([NotNull]Task task, [NotNull]Stream newGenerator);

        /// <summary> Удалить генератор </summary>
        void RemoveGenerator([NotNull]Task task);

        /// <summary> Обновить примечание </summary>
        //TODO: а как же темы?
        void UpdateNote([NotNull]Task task, string note);
    }
}