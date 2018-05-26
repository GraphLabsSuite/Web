using System.Collections.Generic;
using GraphLabs.Site.Logic.IoC;
using GraphLabs.Site.Models.IoC;
using GraphLabs.Site.Utils.IoC;

namespace GraphLabs.Site
{
    /// <summary> Конфигурация необходимых сайту сервисов </summary>
    static class SiteServicesConfiguration
    {
        /// <summary> Специфические реестры для сайта </summary>
        public static IEnumerable<IUnityRegistry> Registries
        {
            get
            {
                yield return new ModelsConfiguration();
            }
        }
    }
}
