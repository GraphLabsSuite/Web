using System;
using System.Net.Security;
using System.ServiceModel;
using GraphLabs.WcfServices.Data;

namespace GraphLabs.WcfServices
{
    /// <summary> Сервис регистрации действий студентов </summary>
    [ServiceContract]
    public interface IUserActionsRegistrator
    {
        /// <summary> Регистрирует действия студента </summary>
        /// <param name="taskId"> Идентификатор модуля-задания </param>
        /// <param name="sessionGuid"> Идентификатор сессии </param>
        /// <param name="actions"> Действия для регистрации </param>
        /// <remarks> От этой штуки зависит GraphLabs.Components </remarks>
        [OperationContract()]
        TaskVariantInfo RegisterUserActions(long taskId, Guid sessionGuid, ActionDescription[] actions);
    }
}