using System;
using System.IO;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.Graphs.Helpers;
using GraphLabs.Site.Logic.Tasks;

namespace GraphLabs.WcfServices
{
    /// <summary> Вспомогательный сервис для отладки заданий на сайте </summary>
    public class TaskDebugHelper : ITaskDebugHelper
    {
        private readonly ILabRepository _labRepository;
        private readonly ITaskManager _taskManager;
        private readonly IDbContextManager _contextManager;
        private readonly ITaskRepository _taskRepository;

        public TaskDebugHelper(ILabRepository labRepository, ITaskManager taskManager, IDbContextManager contextManager,
            ITaskRepository taskRepository)
        {
            _labRepository = labRepository;
            _taskManager = taskManager;
            _contextManager = contextManager;
            _taskRepository = taskRepository;
        }


        /// <summary> Загрузить задание для отладки </summary>
        public int UploadDebugTask(byte[] taskData, byte[] variantData, string email, string password)
        {
            using (_contextManager.BeginTransaction())
            {
                // Загружаем задание
                Task task;
                //using (var stream = new MemoryStream(taskData))
                using (var stream = File.OpenRead("c:\\GraphLabs.Tasks.SCC - 1.xap"))
                {
                    task = _taskManager.UploadTask(stream);
                }
                if (task == null)
                    throw new InvalidOperationException("Провал. Задание с таким именем и версией уже существует.");

                task.Note = "Загружено автоматически сервисом отладки.";

                // Загружаем вариант задания
                var variantInfo = new TaskVariant
                                  {
                                      Data = DebugGraphGenerator.GetSerializedGraph(), //variantData,
                                      GeneratorVersion = "1",
                                      Number = "Debug",
                                      Task = task
                                  };
                _taskRepository.CreateOrUpdateVariant(variantInfo);

                // Создаём лабу
                var now = DateTime.Now;
                var lab = new LabWork
                          {
                              Name = string.Format("{0:d} Отладка задания \"{1}\" (v.{2})", now, task.Name, task.Version),
                              AcquaintanceFrom = now,
                              AcquaintanceTill = now.AddYears(1),
                          };
                _labRepository.SaveLabWork(lab);

                // Создаём вариант
                var labVariant = new LabVariant
                                 {
                                     IntroducingVariant = true,
                                     LabWork = lab,
                                     Number = "Debug",
                                     Version = 1,
                                 };
                _labRepository.SaveLabVariant(labVariant);
            }

            return 0;
        }
    }
}
