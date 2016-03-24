using System;
using System.Diagnostics.Contracts;
using GraphLabs.Dal.Ef;
using GraphLabs.Site.ServicesConfig;
using Microsoft.Practices.Unity;

namespace GraphLabs.Site
{
    /// <summary> �������� '�������' ���������� ������� </summary>
    internal sealed class RequestUnitOfWork
    {
        /// <summary> ��������� </summary>
        public IUnityContainer Container { get; private set; }


        /// <summary> ������ ������� </summary>
        public void OnRequestBeginning()
        {
            Container = IoC.GetChildContainer();
        }

        private void OnRequestEnding()
        {
            // ���������� ��������� � ����� - � �������, �������� �� �������� (���� ������ �� �����)
            Container.Dispose();
            Container = null;
        }

        /// <summary> �������� ���������� ������� </summary>
        public void OnRequestSuccess()
        {
            Contract.Assert(Container != null, "���� �� ������� ���������� ������� ��� ��� ������.");

            // ��������� ��������� � ��������
            Container.Resolve<IChangesTracker>().SaveChanges();

            OnRequestEnding();
        }

        /// <summary> ������ ��� ���������� ������� </summary>
        public void OnRequestFailure()
        {
            Contract.Assert(Container != null, "���� �� ������� ���������� ������� ��� ��� ������.");

            OnRequestEnding();
        }
    }
}