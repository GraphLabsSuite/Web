using System;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;
using Microsoft.Practices.Unity;

namespace GraphLabs.Site.Utils.IoC
{
    /// <summary> Реестр конфигурации Unity </summary>
    [ContractClass(typeof(UnityRegistryContracts))]
    public interface IUnityRegistry
    {
        /// <summary> Сконфигурировать </summary>
        void ConfigureContainer([NotNull]IUnityContainer container);
    }

    /// <summary> Контракты для <see cref="IUnityRegistry"/> </summary>
    [ContractClassFor(typeof(IUnityRegistry))]
    abstract class UnityRegistryContracts : IUnityRegistry
    {
        /// <summary> Сконфигурировать </summary>
        public void ConfigureContainer(IUnityContainer container)
        {
            Contract.Requires<ArgumentNullException>(container != null);
        }
    }
}
