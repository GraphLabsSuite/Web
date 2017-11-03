using System;
using System.ServiceModel;
using GraphLabs.WcfServices.Data;
using System.ServiceModel.Web;

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
        /// <param name="isTaskFinished"> Задание завершено? </param>
        /// <returns> Количество баллов студента </returns>
        /// <remarks> От этой штуки зависит GraphLabs.Components </remarks>
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "action?id={taskId}&sid={sessionGuid}&finished={isTaskFinished}"
            )]
        int? RegisterUserActions(long taskId, Guid sessionGuid, ActionDescription[] actions, bool isTaskFinished = false);
    }
}