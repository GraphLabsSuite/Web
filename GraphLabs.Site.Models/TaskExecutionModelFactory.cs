using System;
using GraphLabs.Tasks.Contract;
using JetBrains.Annotations;

namespace GraphLabs.Site.Models
{
    /// <summary> Фабрика моделей заданий в лабе </summary>
    [UsedImplicitly]
    internal class TaskExecutionModelFactory : ITaskExecutionModelFactory
    {
        private readonly IInitParamsProvider _initParamsProvider;

        public TaskExecutionModelFactory(IInitParamsProvider initParamsProvider)
        {
            _initParamsProvider = initParamsProvider;
        }

        /// <summary> Для ознакомительного режима </summary>
        public TaskExecutionModel CreateForDemoMode(Guid sessionGuid, string taskName, long taskId, long variantId, long labWorkId, Uri taskCompleteRedirect)
        {
            var initParams = InitParams.ForDemoMode(sessionGuid, taskId, variantId, labWorkId, taskCompleteRedirect);

            var model = new TaskExecutionModel
            {
                TaskId = taskId,
                TaskName = taskName,
                InitParams = _initParamsProvider.GetInitParamsString(initParams),
                IsSolved = false
            };

            return model;
        }

        /// <summary> Для контрольного режима </summary>
        public TaskExecutionModel CreateForControlMode(Guid sessionGuid, string taskName, long taskId, long labWorkId, Uri taskCompleteRedirect)
        {
            var initParams = InitParams.ForControlMode(sessionGuid, taskId, labWorkId, taskCompleteRedirect);

            var model = new TaskExecutionModel
            {
                TaskId = taskId,
                TaskName = taskName,
                InitParams = _initParamsProvider.GetInitParamsString(initParams),
                IsSolved = false
            };

            return model;
        }
    }
}