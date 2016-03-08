using System;

namespace GraphLabs.Site.Models
{
    /// <summary> ������� ������� ������� � ���� </summary>
    public interface ITaskExecutionModelFactory
    {
        /// <summary> ��� ���������������� ������ </summary>
        TaskExecutionModel CreateForDemoMode(Guid sessionGuid, string taskName, long taskId, long variantId, long labWorkId);

        /// <summary> ��� ������������ ������ </summary>
        TaskExecutionModel CreateForControlMode(Guid sessionGuid, string taskName, long taskId, long labWorkId);
    }
}