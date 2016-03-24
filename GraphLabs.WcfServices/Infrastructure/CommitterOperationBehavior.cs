using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using GraphLabs.Dal.Ef;

namespace GraphLabs.WcfServices.Infrastructure
{
    /// <summary> Поведение операции - коммитить, если не было ошибок </summary>
    internal class CommitterOperationBehavior : IOperationBehavior
    {
        private readonly IChangesTracker _tracker;

        /// <summary> Поведение операции - коммитить, если не было ошибок </summary>
        public CommitterOperationBehavior(IChangesTracker tracker)
        {
            _tracker = tracker;
        }

        /// <summary> заглушка </summary>
        public void Validate(OperationDescription operationDescription)
        {
        }

        /// <summary> Задекорируем исходный Invoker </summary>
        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.Invoker = new SuccessComitterInvokerDecorator(_tracker, dispatchOperation.Invoker);
        }

        /// <summary> заглушка </summary>
        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }

        /// <summary> заглушка </summary>
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }
    }
}