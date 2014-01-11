using System;
using System.ServiceModel;
using GraphLabs.WcfServices.Data;

namespace GraphLabs.WcfServices
{
    /// <summary> Сервис предоставления данных модулям заданий </summary>
    [ServiceContract]
    public interface ITasksDataService
    {
        /// <summary> Регистрирует начало выполнения задания </summary>
        /// <param name="taskId"> Идентификатор модуля-задания </param>
        /// <param name="sessionGuid"> Идентификатор сессии </param>
        [OperationContract]
        TaskVariantInfo InitialiseTask(long taskId, Guid sessionGuid);


        /// <summary> Регистрирует завершение выполнения задания </summary>
        /// <param name="sessionId"> Идентификатор сессии </param>
        [OperationContract]
        TaskVariantInfo FinishTask(string sessionId);
    }
}
