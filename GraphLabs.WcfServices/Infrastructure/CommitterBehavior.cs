using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using GraphLabs.Dal.Ef;

namespace GraphLabs.WcfServices.Infrastructure
{
    /// <summary> Расширение для контракта, чтобы коммитились изменения </summary>
    internal sealed class CommitterBehavior : IContractBehavior
    {
        private readonly IChangesTracker _tracker;

        /// <summary> Расширение для контракта, чтобы коммитились изменения </summary>
        public CommitterBehavior(IChangesTracker tracker)
        {
            _tracker = tracker;
        }


        /// <summary> заглушка </summary>
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }

        /// <summary> Добавляет необходимое поведение на все операции </summary>
        public void ApplyDispatchBehavior(
            ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            foreach (var operation in contractDescription.Operations)
            {
                operation.Behaviors.Add(new CommitterOperationBehavior(_tracker));
            }
        }


        /// <summary> заглушка </summary>
        public void ApplyClientBehavior(
            ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        /// <summary> заглушка </summary>
        public void AddBindingParameters(
            ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }
    }
}