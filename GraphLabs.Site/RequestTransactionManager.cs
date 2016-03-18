using System;
using System.Diagnostics.Contracts;
using System.Transactions;
using GraphLabs.Dal.Ef;
using GraphLabs.Site.ServicesConfig;
using GraphLabs.Site.Utils.Extensions;
using GraphLabs.Site.Utils.IoC;

namespace GraphLabs.Site
{
    /// <summary> �������� '�������' ���������� ������� </summary>
    internal sealed class RequestTransactionManager
    {
        /// <summary> ��������� </summary>
        public IDependencyResolver Container { get; private set; }

        private TransactionScope _transactionScope;

        /// <summary> ������ ������� </summary>
        public void OnRequestBeginning()
        {
            _transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            });
            Container = IoC.GetChildContainer();
        }

        private void OnRequestEnding()
        {
            // ���������� ��������� � ����� - � �������, �������� �� �������� (���� ������ �� �����)
            Container.Dispose();
            Container = null;
            _transactionScope.Dispose();
            _transactionScope = null;
        }

        /// <summary> �������� ���������� ������� </summary>
        public void OnRequestSuccess()
        {
            Contract.Assert(Container != null && _transactionScope != null, "���� �� ������� ���������� ������� ��� ��� ������.");

            // ��������� ��������� � ��������
            Container.Resolve<IChangesTracker>().SaveChanges();
            _transactionScope.Complete();

            OnRequestEnding();
        }

        /// <summary> ������ ��� ���������� ������� </summary>
        public void OnRequestFailure()
        {
            Contract.Assert(Container != null && _transactionScope != null, "���� �� ������� ���������� ������� ��� ��� ������.");

            OnRequestEnding();
        }
    }
}