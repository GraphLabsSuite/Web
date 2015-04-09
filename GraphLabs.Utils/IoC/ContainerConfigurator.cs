using System;
using Microsoft.Practices.Unity;

namespace GraphLabs.Site.Utils.IoC
{
    /// <summary> Конфигуратор контейнера </summary>
    public abstract class ContainerConfigurator
    {
        /// <summary> Выполнить конфигурацию </summary>
        public abstract void Configure(IUnityContainer container);
    }
}
