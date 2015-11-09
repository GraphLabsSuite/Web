using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Microsoft.Practices.Unity;
using Unity.Wcf;

namespace GraphLabs.WcfServices.Infrastructure
{
    /// <summary> Фабрика сервисов с поддержкой Unity </summary>
	public class WcfServiceFactory : ServiceHostFactory
    {
        private IUnityContainer _container;

        public WcfServiceFactory()
        {
            _container = new UnityContainer();
            IoC.Configure(_container);
        }

        /// <summary> Creates a <see cref="T:System.ServiceModel.ServiceHost"/> for a specified type of service with a specific base address.  </summary>
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return new UnityServiceHost(_container.CreateChildContainer(), serviceType, baseAddresses);
        }
    }    
}