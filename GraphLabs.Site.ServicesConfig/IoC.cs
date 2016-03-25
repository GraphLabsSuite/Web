using System.Collections.Generic;
using System.Linq;
using GraphLabs.Site.Utils.IoC;
using Microsoft.Practices.Unity;

namespace GraphLabs.Site.ServicesConfig
{
    /// <summary> Unity-контейнер </summary>
    public static class IoC
    {
        private static readonly object _containerLock = new object();

        private static IUnityContainer _container;

        /// <summary> Построение корневого контейнера </summary>
        /// <remarks> Применяет блокировку с двойной проверкой, т.к. может быть вызван вразнобой втч из разных потоков </remarks>
        public static void BuildUp(IEnumerable<IUnityRegistry> registies)
        {
            if (_container != null)
            {
                return;
            }

            lock (_containerLock)
            {
                if (_container != null) return;

                var allRegistries = registies.Concat(CommonServicesConfiguration.Registries);
                _container = BuildUnityContainer(allRegistries);
                _container.RegisterInstance(_container, new ExternallyControlledLifetimeManager());
            }
        }

        /// <summary> Построение корневого контейнера </summary>
        public static void BuildUp(IUnityRegistry registry)
        {
            BuildUp(new [] {registry});
        }

        /// <summary> Создать дочерний контейнер </summary>
        public static IUnityContainer GetChildContainer()
        {
            var child = _container.CreateChildContainer();
            child.RegisterInstance(child, new ExternallyControlledLifetimeManager());

            return child;
        }

        private static IUnityContainer BuildUnityContainer(IEnumerable<IUnityRegistry> registies)
        {
            var container = new UnityContainer();

            foreach (var unityRegistry in registies)
            {
                unityRegistry.ConfigureContainer(container);
            }

            return container;
        }
    }
}