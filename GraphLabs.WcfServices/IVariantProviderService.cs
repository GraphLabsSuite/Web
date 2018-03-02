using System;
using System.ServiceModel;
using GraphLabs.WcfServices.Data;
using System.ServiceModel.Web;

namespace GraphLabs.WcfServices
{
    /// <summary> Сервис предоставления данных модулям заданий </summary>
    [ServiceContract]
    public interface IVariantProviderService
    {
        /// <summary> Регистрирует начало выполнения задания </summary>
        /// <param name="taskId"> Идентификатор модуля-задания </param>
        /// <param name="sessionGuid"> Идентификатор сессии </param>
        /// <returns> Вариант задания </returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
            UriTemplate = "variant?id={taskId}&sid={sessionGuid}"
            )]
        TaskVariantDto GetVariant(long taskId, Guid sessionGuid);
    }
}
