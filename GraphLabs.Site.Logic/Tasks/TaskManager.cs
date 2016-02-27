using System;
using System.IO;
using System.Linq;
using GraphLabs.DomainModel.EF;
using GraphLabs.DomainModel.EF.Contexts;
using GraphLabs.DomainModel.EF.Utils;
using GraphLabs.Site.Utils.XapProcessor;

namespace GraphLabs.Site.Logic.Tasks
{
    /// <summary> Менеджер заданий </summary>
    public class TaskManager : ITaskManager
    {
        private readonly ITasksContext _tasksCtx;
        private readonly IXapProcessor _xapProcessor;

        /// <summary> Менеджер заданий </summary>
        public TaskManager(
            ITasksContext tasksCtx,
            IXapProcessor xapProcessor)
        {
            _tasksCtx = tasksCtx;
            _xapProcessor = xapProcessor;
        }

        /// <summary> Загрузить задание </summary>
        public Task UploadTask(Stream stream)
        {
            return UploadTaskInternal(stream, false);
        }

        /// <summary> Загрузить задание и прописать в название время загрузки </summary>
        /// <remarks> Для отладочных целей </remarks>
        public Task UploadTaskWithTimestamp(Stream stream)
        {
            return UploadTaskInternal(stream, true);
        }

        /// <summary> Загрузить задание </summary>
        private Task UploadTaskInternal(Stream stream, bool appendTimestamp)
        {
            var newTask = CreateFromXap(stream);
            if (newTask == null)
                throw new ArgumentException("Не удалось распознать модуль-задание.");

            if (appendTimestamp)
            {
                newTask.Name = $"{newTask.Name} ({DateTime.Now:u})";
            }

            var sameTaskExists = _tasksCtx.Tasks.Any(t => t.Name == newTask.Name && t.Version == newTask.Version);
            if (!sameTaskExists)
            {
                _tasksCtx.Tasks.Add(newTask);
            }
            else
            {
                //TODO: здесь кидать исключение о дубликате (лучше ResultOrError)
                return null;
            }
            return newTask;
        }

        /// <summary> Создаёт экземпляр Task по xap'у </summary>
        /// <param name="stream">xap</param>
        /// <returns>Null, если xap был кривой</returns>
        private Task CreateFromXap(Stream stream)
        {
            var info = _xapProcessor.Parse(stream);

            if (info == null)
                return null;

            var newTask = new Task
            {
                Name = info.Name,
                Sections = info.Sections,
                VariantGenerator = null,
                Note = null,
                Version = info.Version,
                Xap = stream.ReadToEnd()
            };

            return newTask;
        }

        /// <summary> Установить заданию генератор </summary>
        public void SetGenerator(Task task, Stream newGenerator)
        {
            // Убедимся, что хотя бы формат правильный (xap, и нам удалось вытащить информацию о нём)
            if (_xapProcessor.Parse(newGenerator) != null)
            {
                // Наконец, сохраним.
                task.VariantGenerator = newGenerator.ReadToEnd();
            }
            else
            {
                throw new ArgumentException("Не удалось распознать генератор.");
            }
        }

        /// <summary> Обновить примечание </summary>
        //TODO: а как же темы?
        public void UpdateNote(Task task, string note)
        {
            task.Note = note;
        }
        
        /// <summary> Удалить генератор </summary>
        public void RemoveGenerator(Task task)
        {
            task.VariantGenerator = null;
        }
    }
}
