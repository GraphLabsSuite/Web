using System;
using System.IO;
using System.ServiceModel;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Site.Logic.Tasks;
using GraphLabs.Site.Utils;

namespace GraphLabs.WcfServices.DebugTaskUploader
{
    /// <summary> Вспомогательный сервис для отладки заданий на сайте </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)] 
    public class DebugTaskUploader : IDebugTaskUploader
    {
        private readonly ITaskManager _taskManager;
        private readonly IOperationContextFactory<IGraphLabsContext> _operationFactory;

        public DebugTaskUploader(
            ITaskManager taskManager,
            IOperationContextFactory<IGraphLabsContext> operationFactory)
        {
            _taskManager = taskManager;
            _operationFactory = operationFactory;
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
            Task taskPoco;
            using (var stream = new MemoryStream(taskData))
            {
                taskPoco = _taskManager.UploadTaskWithTimestamp(stream);
            }
            if (taskPoco == null)
                throw new InvalidOperationException("Провал. Задание с таким именем и версией уже существует.");

            using (var operation = _operationFactory.Create())
            {
                var task = operation.DataContext.Factory.Create<Task>();
                task.Name = taskPoco.Name;
                task.VariantGenerator = null;
                task.Sections = taskPoco.Sections;
                task.Version = taskPoco.Version;
                task.Xap = taskPoco.Xap;
                task.Note = "Загружено автоматически сервисом отладки.";

                // Загружаем вариант задания
                var taskVariant = operation.DataContext.Factory.Create<TaskVariant>();
                taskVariant.Data = variantData;
                taskVariant.GeneratorVersion = "1";
                taskVariant.Number = "Debug";
                taskVariant.Task = task;

                // Создаём лабу
                var now = DateTime.Now;
                var lab = operation.DataContext.Factory.Create<LabWork>();
                lab.Name = $"Отладка модуля \"{task.Name}\"";
                lab.AcquaintanceFrom = now.Date;
                lab.AcquaintanceTill = now.Date.AddDays(7);

                // Добавляем задание в лабу
                var labEntry = operation.DataContext.Factory.Create<LabEntry>();
                labEntry.LabWork = lab;
                labEntry.Order = 0;
                labEntry.Task = task;

                // Создаём вариант
                var labVariant = operation.DataContext.Factory.Create<LabVariant>();
                labVariant.IntroducingVariant = true;
                labVariant.LabWork = lab;
                labVariant.Number = "Debug";
                labVariant.Version = 1;
                labVariant.TaskVariants = new[] {taskVariant};

                operation.Complete();

                return new DebugTaskData
                {
                    LabWorkId = lab.Id,
                    LabVariantId = labVariant.Id
                };
            }
        }
    }
}
