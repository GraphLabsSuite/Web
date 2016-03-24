using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using GraphLabs.Site.ServicesConfig;
using Unity.Wcf;

namespace GraphLabs.WcfServices.Infrastructure
{
    /// <summary> ������� �������� � ���������� Unity </summary>
	public class WcfServiceFactory : ServiceHostFactory
    {
        /// <summary> ������� �������� � ���������� Unity </summary>
        public WcfServiceFactory()
        {
            IoC.BuildUp(new WcfRegistry());
        }

        /// <summary> Creates a <see cref="T:System.ServiceModel.ServiceHost"/> for a specified type of service with a specific base address.  </summary>
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            // ��� ������ ������ �������� �������� ���������� - �� � ���� �� � ���
            return new UnityServiceHost(IoC.GetChildContainer(), serviceType, baseAddresses);
        }
    }
}