using System;

namespace GraphLabs.Site.Models
{
    /// <summary> Фабрика моделей заданий в лабе </summary>
    public interface ITaskExecutionModelFactory
    {
        /// <summary> Для ознакомительного режима </summary>
        TaskExecutionModel CreateForDemoMode(Guid sessionGuid, string taskName, long taskId, long variantId, long labWorkId);

        /// <summary> Для контрольного режима </summary>
        TaskExecutionModel CreateForControlMode(Guid sessionGuid, string taskName, long taskId, long labWorkId);
    }
}