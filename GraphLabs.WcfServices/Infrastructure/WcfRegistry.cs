using System;
using System.ServiceModel.Description;
using GraphLabs.DomainModel;
using GraphLabs.Site.Utils;
using GraphLabs.Site.Utils.IoC;
using GraphLabs.WcfServices.Data;
using GraphLabs.WcfServices.DebugTaskUploader;
using Microsoft.Practices.Unity;

namespace GraphLabs.WcfServices.Infrastructure
{
    /// <summary> Реестр конфигурации Unity для WCF </summary>
    sealed class WcfRegistry : IUnityRegistry
    {
        /// <summary> Сконфигурировать </summary>
        public void ConfigureContainer(IUnityContainer container)
        {
            container.RegisterInstance(CreateMapper());
            container.RegisterType<IVariantProviderService, VariantProviderService>();
            container.RegisterType<IUserActionsRegistrator, UserActionsRegistrator>();
            container.RegisterType<IVariantGenService, VariantGenService>();
            container.RegisterType<IDebugTaskUploader, DebugTaskUploader.DebugTaskUploader>();
        }

        private static Mapper CreateMapper()
        {
            var mapper = new Mapper();

            // Сюда можно положить специфичные для WCF маппинги
            mapper.CreateMap<TaskVariant, TaskVariantDto>();
            return mapper;
        }
    }
}
