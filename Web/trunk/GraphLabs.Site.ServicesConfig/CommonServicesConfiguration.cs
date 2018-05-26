using System.Collections.Generic;
using GraphLabs.Dal.Ef.IoC;
using GraphLabs.Site.Logic.IoC;
using GraphLabs.Site.Utils.IoC;

namespace GraphLabs.Site.ServicesConfig
{
    /// <summary> Конфигурация общих сервисов </summary>
    internal static class CommonServicesConfiguration
    {
        /// <summary> Реестры общих сервисов </summary>
        public static IEnumerable<IUnityRegistry> Registries
        {
            get
            {
                yield return new SiteLogicConfiguration();
                yield return new DalConfiguration();
            }
        }
    }
}