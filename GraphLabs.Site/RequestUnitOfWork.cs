using System;
using System.Diagnostics.Contracts;
using GraphLabs.Dal.Ef;
using GraphLabs.Site.ServicesConfig;
using GraphLabs.Site.Utils.Extensions;
using GraphLabs.Site.Utils.IoC;

namespace GraphLabs.Site
{
    /// <summary> Менеджер 'главной' транзакции запроса </summary>
    internal sealed class RequestUnitOfWork
    {
        /// <summary> Контейнер </summary>
        public IDependencyResolver Container { get; private set; }


        /// <summary> Начало запроса </summary>
        public void OnRequestBeginning()
        {
            Container = IoC.GetChildContainer();
        }

        private void OnRequestEnding()
        {
            // уничтожаем контейнер и скоуп - в порядке, обратном их созданию (хотя кажись не важно)
            Container.Dispose();
            Container = null;
        }

        /// <summary> Успешное выполнение запроса </summary>
        public void OnRequestSuccess()
        {
            Contract.Assert(Container != null, "Один из методов завершения запроса уже был вызван.");

            // Сохраняем изменения и коммитим
            Container.Resolve<IChangesTracker>().SaveChanges();

            OnRequestEnding();
        }

        /// <summary> Ошибка при выполнении запроса </summary>
        public void OnRequestFailure()
        {
            Contract.Assert(Container != null, "Один из методов завершения запроса уже был вызван.");

            OnRequestEnding();
        }
    }
}