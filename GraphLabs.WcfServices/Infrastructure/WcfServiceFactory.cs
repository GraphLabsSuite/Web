using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using GraphLabs.Site.ServicesConfig;
using Unity.Wcf;

namespace GraphLabs.WcfServices.Infrastructure
{
    /// <summary> Фабрика сервисов с поддержкой Unity </summary>
	public sealed class WcfServiceFactory : ServiceHostFactory
    {
        /// <summary> Фабрика сервисов с поддержкой Unity </summary>
        public WcfServiceFactory()
        {
            IoC.BuildUp(new WcfRegistry());
        }

        /// <summary> Creates a <see cref="T:System.ServiceModel.ServiceHost"/> for a specified type of service with a specific base address.  </summary>
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            // оно внутри создаёт дочерние дочерние контейнеры - да и чёрт бы с ним
            return new UnityServiceHost(IoC.GetChildContainer(), serviceType, baseAddresses);
        }
    }
}