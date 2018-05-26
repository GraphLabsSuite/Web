using System;
using System.IO;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Site.Logic.XapParsing;
using GraphLabs.Site.Utils.Extensions;
using System.Text;

namespace GraphLabs.Site.Logic.Tasks
{
    /// <summary> plain-old-clr-object для Task,т.к. лень ночью писать dto - тут ещё будут переделки </summary>
    public class TaskPoco : Task
    {
        public byte[] Xap { get; set; }
    }


    /// <summary> Менеджер заданий </summary>
    public class TaskManager : ITaskManager
    {
        private readonly IEntityQuery _query;
        private readonly IXapProcessor _xapProcessor;

        /// <summary> Менеджер заданий </summary>
        public TaskManager(IEntityQuery query, IXapProcessor xapProcessor)
        {
            _query = query;
            _xapProcessor = xapProcessor;
        }

        /// <summary> Загрузить задание </summary>
        public TaskPoco UploadTask(Stream stream)
        {
            return UploadTaskInternal(stream, false);
        }

        public TaskPoco UploadReactTask(string scriptName)
        {
            string taskName = getReactTaskName(scriptName);
            string taskVersion = getReactTaskVersion(scriptName);
            var sameTaskExists = _query.OfEntities<Task>().Any(t => t.Name == taskName && t.Version == taskVersion);

            if (!sameTaskExists)
            {
                string xapStr = "React module - xap is redundant";
                var newTask = new TaskPoco
                {
                    Name = taskName,
                    Sections = null,
                    VariantGenerator = null,
                    Note = null,
                    Version = taskVersion,
                    Xap = Encoding.ASCII.GetBytes(xapStr)
                };

                return newTask;
            }
            else
            {
                //TODO: здесь кидать исключение о дубликате
                return null;
            }
        }
        

        private string getReactTaskName(string scriptName)
        {
            return getReactTaskNameWithVersion(scriptName).Split('-')[0];
        }

        private string getReactTaskVersion(string scriptName)
        {
            return getReactTaskNameWithVersion(scriptName).Split('-')[1];
        }

        private string getReactTaskNameWithVersion(string scriptName)
        {
            return scriptName.Split(new[] { ".min." }, StringSplitOptions.None)[0];
        }

        /// <summary> Загрузить задание и прописать в название время загрузки </summary>
        /// <remarks> Для отладочных целей </remarks>
        public TaskPoco UploadTaskWithTimestamp(Stream stream)
        {
            return UploadTaskInternal(stream, true);
        }

        /// <summary> Загрузить задание </summary>
        private TaskPoco UploadTaskInternal(Stream stream, bool appendTimestamp)
        {
            var info = _xapProcessor.Parse(stream);
            if (info == null)
                throw new ArgumentException("Не удалось распознать модуль-задание.");

            var name = appendTimestamp
                ? $"{info.Name} ({DateTime.Now:u})"
                : info.Name;

            var sameTaskExists = _query.OfEntities<Task>().Any(t => t.Name == name && t.Version == info.Version);

            if (!sameTaskExists)
            {
                var newTask = new TaskPoco
                {
                    Name = name,
                    Sections = info.Sections,
                    VariantGenerator = null,
                    Note = null,
                    Version = info.Version,
                    Xap = stream.ReadToEnd()
                };

                return newTask;
            }
            else
            {
                //TODO: здесь кидать исключение о дубликате (лучше ResultOrError)
                return null;
            }
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
