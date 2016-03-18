using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;

namespace GraphLabs.Site.Utils.IoC
{
    /// <summary> IoC-контейнер </summary>
    [ContractClass(typeof(DependencyResolverContracts))]
    public interface IDependencyResolver : IDisposable
    {
        /// <summary> Получить сконфигурированную зависимость по-умолчанию </summary>
        [NotNull]
        object Resolve(Type serviceType);

        /// <summary> Получить все сконфигурированные зависимости </summary>
        [NotNull]
        IEnumerable<object> ResolveAll(Type serviceType);
    }

    /// <summary> Контакты для <see cref="IDependencyResolver"/> </summary>
    [ContractClassFor(typeof(IDependencyResolver))]
    abstract class DependencyResolverContracts : IDependencyResolver
    {
        /// <summary> Получить сконфигурированную зависимость по-умолчанию </summary>
        public object Resolve([NotNull]Type serviceType)
        {
            Contract.Requires<ArgumentNullException>(serviceType != null);
            Contract.Ensures(Contract.Result<object>() != null);
            Contract.Ensures(serviceType.IsInstanceOfType(Contract.Result<object>()));

            return default(object);
        }

        /// <summary> Получить все сконфигурированные зависимости </summary>
        public IEnumerable<object> ResolveAll([NotNull]Type serviceType)
        {
            Contract.Requires<ArgumentNullException>(serviceType != null);
            Contract.Ensures(Contract.Result<IEnumerable<object>>() != null);
            Contract.Ensures(Contract.ForAll(Contract.Result<IEnumerable<object>>(), serviceType.IsInstanceOfType));

            return default(object[]);
        }

        /// <summary> Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
        public abstract void Dispose();
    }
}
