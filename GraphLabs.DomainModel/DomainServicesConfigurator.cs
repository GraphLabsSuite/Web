using System;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.Site.Utils.IoC;
using Microsoft.Practices.Unity;

namespace GraphLabs.DomainModel
{
    /// <summary> Конфигуратор доменных сервисов </summary>
    public class DomainServicesConfigurator : ContainerConfigurator
    {
        /// <summary> Выполнить конфигурацию </summary>
        public override void Configure(IUnityContainer container)
        {
            container.RegisterType<GraphLabsContext>(new HierarchicalLifetimeManager());
            var graphLabsContextFactory = new InjectionFactory(c => c.Resolve<GraphLabsContext>());

            container.RegisterType<INewsContext>(new HierarchicalLifetimeManager(), graphLabsContextFactory);
        }
    }
}
