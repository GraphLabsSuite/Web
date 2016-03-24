using System;
using System.ServiceModel.Dispatcher;
using GraphLabs.Dal.Ef;

namespace GraphLabs.WcfServices.Infrastructure
{
    /// <summary> ��������� ��� Invoker'�. ��������� ���������, ���� �� ���� ������ </summary>
    internal sealed class SuccessComitterInvokerDecorator : IOperationInvoker
    {
        private readonly IChangesTracker _tracker;
        private readonly IOperationInvoker _underlayingInvoker;

        /// <summary> ��������� ��� Invoker'�. ��������� ���������, ���� �� ���� ������ </summary>
        public SuccessComitterInvokerDecorator(IChangesTracker tracker, IOperationInvoker underlayingInvoker)
        {
            _tracker = tracker;
            _underlayingInvoker = underlayingInvoker;
        }

        /// <summary> ������� ������ ��� ������� ���������� </summary>
        public object[] AllocateInputs()
        {
            return _underlayingInvoker.AllocateInputs();
        }

        /// <summary> ���������� </summary>
        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            var result = _underlayingInvoker.Invoke(instance, inputs, out outputs);
            _tracker.SaveChanges();

            return result;
        }

        /// <summary> ������ ���������� </summary>
        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            return _underlayingInvoker.InvokeBegin(instance, inputs, callback, state);
        }

        /// <summary> ��������� ���������� </summary>
        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            var retValue = _underlayingInvoker.InvokeEnd(instance, out outputs, result);
            if (result.IsCompleted)
            {
                _tracker.SaveChanges();
            }
            return retValue;
        }

        /// <summary> �������� �� ������ ����������? </summary>
        public bool IsSynchronous => _underlayingInvoker.IsSynchronous;
    }
}