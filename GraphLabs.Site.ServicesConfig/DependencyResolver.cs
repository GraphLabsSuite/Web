using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using GraphLabs.Site.Utils.IoC;
using Microsoft.Practices.Unity;

namespace GraphLabs.Site.ServicesConfig
{
    /// <summary> Read-only ������ ��� ����������� </summary>
    internal sealed class DependencyResolver : IDependencyResolver
    {
        private readonly IUnityContainer _container;

        public DependencyResolver(IUnityContainer container)
        {
            Contract.Requires<ArgumentNullException>(container != null);

            _container = container;
        }

        /// <summary> �������� ������������������ ����������� ��-��������� </summary>
        public object Resolve(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        /// <summary> �������� ��� ������������������ ����������� </summary>
        public IEnumerable<object> ResolveAll(Type serviceType)
        {
            return _container.ResolveAll(serviceType);
        }

        /// <summary> Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
        public void Dispose()
        {
            _container.Dispose();
        }
    }
}