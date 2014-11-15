using System;
using System.IO;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.DomainModel.Utils;
using GraphLabs.Site.Utils.XapProcessor;

namespace GraphLabs.Site.Logic.Tasks
{
    /// <summary> Менеджер заданий </summary>
    public class TaskManager : ITaskManager
    {
        private readonly ITransactionManager _transactionManager;
        private readonly ITaskRepository _taskRepository;
        private readonly IXapProcessor _xapProcessor;

        /// <summary> Менеджер заданий </summary>
        public TaskManager(
            ITransactionManager transactionManager,
            ITaskRepository taskRepository,
            IXapProcessor xapProcessor)
        {
            _transactionManager = transactionManager;
            _taskRepository = taskRepository;
            _xapProcessor = xapProcessor;
        }

        /// <summary> Загрузить задание </summary>
        public Task UploadTask(Stream stream)
        {

            var newTask = CreateFromXap(stream);
            if (newTask == null)
                throw new ArgumentException("Не удалось распознать модуль-задание.");

            var sameTaskExists = _taskRepository.IsAnySameTask(newTask.Name, newTask.Version);
            if (!sameTaskExists)
            {
                _taskRepository.Insert(newTask);
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
            CheckTaskIsAttachedToContext(task);

            // Убедимся, что хотя бы формат правильный (xap, и нам удалось вытащить информацию о нём)
            if (_xapProcessor.Parse(newGenerator) != null)
            {
                // Наконец, сохраним.
                using (_transactionManager.BeginTransaction_BUGGED())
                {
                    task.VariantGenerator = newGenerator.ReadToEnd();
                }
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
            CheckTaskIsAttachedToContext(task);

            using (_transactionManager.BeginTransaction_BUGGED())
            {
                task.Note = note;
            }
        }
        
        /// <summary> Удалить генератор </summary>
        public void RemoveGenerator(Task task)
        {
            CheckTaskIsAttachedToContext(task);

            using (_transactionManager.BeginTransaction_BUGGED())
            {
                task.VariantGenerator = null;
            }
        }

        private void CheckTaskIsAttachedToContext(Task task)
        {
            if (!_transactionManager.IsEntityAttached(task))
            {
                throw new ArgumentException("Сущность не прикреплена к текущему контексту.", "task");
            }
        }
    }
}
