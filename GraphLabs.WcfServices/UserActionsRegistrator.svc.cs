using System;
using System.ServiceModel.Activation;
using GraphLabs.WcfServices.Data;

namespace GraphLabs.WcfServices
{
    /// <summary> Сервис предоставления данных модулям заданий </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class UserActionsRegistrator : IUserActionsRegistrator
    {
        public TaskVariantInfo RegisterUserActions(long taskId, Guid sessionGuid, ActionDescription[] actions)
        {
            throw new NotImplementedException();
        }
    }
}