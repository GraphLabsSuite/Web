using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using GraphLabs.Dal.Ef;

namespace GraphLabs.WcfServices.Infrastructure
{
    /// <summary> ���������� ��� ���������, ����� ����������� ��������� </summary>
    internal sealed class CommitterBehavior : IContractBehavior
    {
        private readonly IChangesTracker _tracker;

        /// <summary> ���������� ��� ���������, ����� ����������� ��������� </summary>
        public CommitterBehavior(IChangesTracker tracker)
        {
            _tracker = tracker;
        }


        /// <summary> �������� </summary>
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }

        /// <summary> ��������� ����������� ��������� �� ��� �������� </summary>
        public void ApplyDispatchBehavior(
            ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            foreach (var operation in contractDescription.Operations)
            {
                operation.Behaviors.Add(new CommitterOperationBehavior(_tracker));
            }
        }


        /// <summary> �������� </summary>
        public void ApplyClientBehavior(
            ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        /// <summary> �������� </summary>
        public void AddBindingParameters(
            ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }
    }
}