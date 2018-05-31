using System;
using System.Diagnostics.Contracts;
using GraphLabs.Dal.Ef;
using GraphLabs.Site.ServicesConfig;
using Microsoft.Practices.Unity;

namespace GraphLabs.Site
{
    /// <summary> Менеджер 'главной' транзакции запроса </summary>
    internal sealed class RequestUnitOfWork
    {
        /// <summary> Контейнер </summary>
        public IUnityContainer Container { get; private set; }


        /// <summary> Начало запроса </summary>
        public void OnRequestBeginning()
        {
            Container = IoC.GetChildContainer();
        }

        private void OnRequestEnding()
        {
            Container.Dispose();
            Container = null;
        }

        /// <summary> Успешное выполнение запроса </summary>
        public void OnRequestSuccess()
        {
            Guard.IsTrueAssertion("Один из методов завершения запроса уже был вызван.",Container != null );

            Container.Resolve<IChangesTracker>().SaveChanges();

            OnRequestEnding();
        }

        /// <summary> Ошибка при выполнении запроса </summary>
        public void OnRequestFailure()
        {
            Guard.IsTrueAssertion("Один из методов завершения запроса уже был вызван.", Container != null);
            OnRequestEnding();
        }
    }
}