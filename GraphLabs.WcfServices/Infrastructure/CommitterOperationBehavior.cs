using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using GraphLabs.Dal.Ef;

namespace GraphLabs.WcfServices.Infrastructure
{
    /// <summary> ��������� �������� - ���������, ���� �� ���� ������ </summary>
    internal class CommitterOperationBehavior : IOperationBehavior
    {
        private readonly IChangesTracker _tracker;

        /// <summary> ��������� �������� - ���������, ���� �� ���� ������ </summary>
        public CommitterOperationBehavior(IChangesTracker tracker)
        {
            _tracker = tracker;
        }

        /// <summary> �������� </summary>
        public void Validate(OperationDescription operationDescription)
        {
        }

        /// <summary> ������������ �������� Invoker </summary>
        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.Invoker = new SuccessComitterInvokerDecorator(_tracker, dispatchOperation.Invoker);
        }

        /// <summary> �������� </summary>
        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }

        /// <summary> �������� </summary>
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }
    }
}