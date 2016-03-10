using System.Collections.Generic;
using GraphLabs.DomainModel.EF.IoC;
using GraphLabs.Site.Logic.IoC;
using GraphLabs.Site.Models.IoC;
using GraphLabs.Site.Utils.IoC;
using Microsoft.Practices.Unity;

namespace GraphLabs.Site.ServicesConfig
{
    /// <summary> Unity-контейнер </summary>
    public static class IoC
    {
        private static IUnityContainer _container;

        /// <summary> Построение корневого контейнера. Дёргать на Application_Start в Global.asax </summary>
        public static void BuildUp()
        {
            if (_container == null)
            {
                _container = BuildUnityContainer();
            }
        }

        /// <summary> Создать дочерний контейнер </summary>
        public static IUnityContainer GetChildContainer()
        {
            return _container.CreateChildContainer();
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            RegisterTypes(container);
            return container;
        }

        private static IEnumerable<IUnityRegistry> GetRegistries()
        {
            yield return new DomainModelEfConfiguration();
            yield return new SiteLogicConfiguration();
            yield return new ModelsConfiguration();
        }

        /// <summary> Регистрируем компоненты </summary>
        private static void RegisterTypes(IUnityContainer container)
        {
            foreach (var unityRegistry in GetRegistries())
            {
                unityRegistry.ConfigureContainer(container);
            }
        }
    }
}