using System;
using Microsoft.Practices.Unity;

namespace GraphLabs.Site.Utils.IoC
{
    /// <summary> Реестр конфигурации Unity </summary>
    public interface IUnityRegistry
    {
        /// <summary> Сконфигурировать </summary>
        void ConfigureContainer(IUnityContainer container);
    }
}
