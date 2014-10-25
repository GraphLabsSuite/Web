using System;
using System.IO;
using GraphLabs.DomainModel;
using GraphLabs.Graphs.Helpers;
using GraphLabs.Site.Logic.Tasks;

namespace GraphLabs.WcfServices
{
    /// <summary> Вспомогательный сервис для отладки заданий на сайте </summary>
    public class TaskDebugHelper : ITaskDebugHelper
    {
        private readonly ITaskManager _taskManager;
        private readonly ITaskVariantManager _taskVariantManager;

        public TaskDebugHelper(LabsLogic labsLogic, ITaskManager taskManager, ITaskVariantManager taskVariantManager)
        {
            _labsLogic = labsLogic;
            _taskManager = taskManager;
            _taskVariantManager = taskVariantManager;
        }


        /// <summary> Загрузить задание для отладки </summary>
        public int UploadDebugTask(byte[] taskData, byte[] variantData, string email, string password)
        {
            // Загружаем задание
            ITask task;
            //using (var stream = new MemoryStream(taskData))
            using (var stream = File.OpenRead("c:\\GraphLabs.Tasks.SCC - 1.xap"))
            {
                task = _taskManager.UploadTask(stream, "Загружено автоматически сервисом отладки.");
            }
            if (task == null)
                throw new InvalidOperationException("Провал. Задание с таким именем и версией уже существует.");
            
            // Загружаем вариант задания
            var variantInfo = new TaskVariantInfo
            {
                Data = DebugGraphGenerator.GetSerializedGraph(),//variantData,
                GeneratorVersion = "1",
                Number = "Debug",
                TaskId = task.Id
            };
            _taskVariantManager.CreateOrUpdateVariant(variantInfo);

            // Создаём лабу
            var now = DateTime.Now;
            var lab = _labsLogic.CreateOrGetLabWorkDependingOnId();
            lab.Name = string.Format("{0:d} Отладка задания \"{1}\" (v.{2})", now, task.Name, task.Version);
            lab.AcquaintanceFrom = now;
            lab.AcquaintanceTill = now.AddYears(1);
            _labsLogic.SaveNewLabWork(lab);

            // Создаём вариант
            var labVariant = _labsLogic.CreateOrGetLabVariantDependingOnId();
            labVariant.IntroducingVariant = true;
            labVariant.LabWork = lab;
            labVariant.Number = "Debug";
            labVariant.Version = 1;
            _labsLogic.SaveNewLabVariant(labVariant);

            return 0;
        }
    }
}
