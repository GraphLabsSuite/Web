using System;
using System.IO;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.Dal.Ef;
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
            var taskVariant = _tasksCtx.TaskVariants.CreateNew();
            taskVariant.Data = variantData;
            taskVariant.GeneratorVersion = "1";
            taskVariant.Number = "Debug";
            taskVariant.Task = task;

            // Создаём лабу
            var now = DateTime.Now;
            var lab = _labsCtx.LabWorks.CreateNew();
            lab.Name = $"Отладка модуля \"{task.Name}\"";
            lab.AcquaintanceFrom = now.Date;
            lab.AcquaintanceTill = now.Date.AddDays(7);

            // Добавляем задание в лабу
            var labEntry = _labsCtx.LabEntries.CreateNew();
            labEntry.LabWork = lab;
            labEntry.Order = 0;
            labEntry.Task = task;

            // Создаём вариант
            var labVariant = _labsCtx.LabVariants.CreateNew();
            labVariant.IntroducingVariant = true;
            labVariant.LabWork = lab;
            labVariant.Number = "Debug";
            labVariant.Version = 1;
            labVariant.TaskVariants = new[] {taskVariant};

            _changesTracker.SaveChanges();

            return new DebugTaskData
            {
                LabWorkId = lab.Id,
                LabVariantId = labVariant.Id
            };
        }
    }
}
