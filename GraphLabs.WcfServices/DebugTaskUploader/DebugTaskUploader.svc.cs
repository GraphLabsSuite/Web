using System;
using System.IO;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.EF;
using GraphLabs.Site.Logic.Tasks;
using GraphLabs.Site.Utils;

namespace GraphLabs.WcfServices.DebugTaskUploader
{
    /// <summary> Вспомогательный сервис для отладки заданий на сайте </summary>
    public class DebugTaskUploader : IDebugTaskUploader
    {
        private readonly ILabWorksContext _labsCtx;
        private readonly ITasksContext _tasksCtx;
        private readonly ITaskManager _taskManager;
        private readonly IChangesTracker _changesTracker;

        public DebugTaskUploader(
            ILabWorksContext labsCtx,
            ITasksContext tasksCtx,
            ITaskManager taskManager,
            IChangesTracker changesTracker)
        {
            _labsCtx = labsCtx;
            _tasksCtx = tasksCtx;
            _taskManager = taskManager;
            _changesTracker = changesTracker;
        }

        /// <summary> Загрузить задание для отладки </summary>
        public DebugTaskData UploadDebugTask(byte[] taskData, byte[] variantData)
        {
            if (!WorkingMode.IsDebug())
            {
                throw new InvalidOperationException(
                    "Создание тестовых вариантов возможно только при работе в тестовом режиме.");
            }

            // Загружаем задание
            Task task;
            using (var stream = new MemoryStream(taskData))
            {
                task = _taskManager.UploadTaskWithTimestamp(stream);
            }
            if (task == null)
                throw new InvalidOperationException("Провал. Задание с таким именем и версией уже существует.");

            task.Note = "Загружено автоматически сервисом отладки.";

            // Загружаем вариант задания
            var taskVariant = new TaskVariant
            {
                Data = variantData,
                GeneratorVersion = "1",
                Number = "Debug",
                Task = task
            };
            _tasksCtx.TaskVariants.Add(taskVariant);

            // Создаём лабу
            var now = DateTime.Now;
            var lab = new LabWork
            {
                Name = $"Отладка модуля \"{task.Name}\"",
                AcquaintanceFrom = now.Date,
                AcquaintanceTill = now.Date.AddDays(7),
            };
            _labsCtx.LabWorks.Add(lab);

            // Добавляем задание в лабу
            var labEntry = new LabEntry()
            {
                LabWork = lab,
                Order = 0,
                Task = task
            };
            _labsCtx.LabEntries.Add(labEntry);

            // Создаём вариант
            var labVariant = new LabVariant
            {
                IntroducingVariant = true,
                LabWork = lab,
                Number = "Debug",
                Version = 1,
                TaskVariants = new [] { taskVariant }
            };
            _labsCtx.LabVariants.Add(labVariant);

            _changesTracker.SaveChanges();

            return new DebugTaskData
            {
                LabWorkId = lab.Id,
                LabVariantId = labVariant.Id
            };
        }
    }
}
