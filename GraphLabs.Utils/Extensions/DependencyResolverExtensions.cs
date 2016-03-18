using System;
using System.Linq;
using GraphLabs.Site.Utils.IoC;
using JetBrains.Annotations;

namespace GraphLabs.Site.Utils.Extensions
{
    /// <summary> Расширения для <see cref="IDependencyResolver"/> </summary>
    public static class DependencyResolverExtensions
    {
        /// <summary> Получить сконфигурированную зависимость по-умолчанию </summary>
        [NotNull]
        public static TService Resolve<TService>(this IDependencyResolver resolver)
        {
            return (TService)resolver.Resolve(typeof(TService));
        }

        /// <summary> Получить все сконфигурированные зависимости </summary>
        [NotNull]
        public static TService[] ResolveAll<TService>(this IDependencyResolver resolver)
        {
            return resolver
                .ResolveAll(typeof(TService))
                .Cast<TService>()
                .ToArray();

        }
    }
}
