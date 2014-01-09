using System;
using System.Collections.Generic;

namespace GraphLabs.DomainModel.Services
{
    /// <summary> ServiceLocator </summary>
    public class ServiceLocator
    {
        #region Singletone

        /// <summary> Единственный экземпляр </summary>
        public static ServiceLocator Locator
        {
            get { return _locator ?? (_locator = new ServiceLocator()); }
        }
        private static ServiceLocator _locator;

        #endregion

        /// <summary> Инициализация </summary>
        private ServiceLocator()
        {
            const int SERVICES_COUNT = 1;

            _servicesTypes = new Dictionary<Type, Type>(SERVICES_COUNT);
            _services = new Dictionary<Type, object>(SERVICES_COUNT);

            _servicesTypes.Add(typeof(ISystemDateService), typeof(SystemDateService));
        }
        
        /// <summary> Контракт -> Реализующий тип </summary>
        private readonly IDictionary<Type, Type> _servicesTypes;

        /// <summary> Контракт -> Экземпляр сервиса </summary>
        private readonly IDictionary<Type, object> _services;

        /// <summary> Возвращает экземпляр сервиса </summary>
        public T Get<T>()
        {
            var contract = typeof(T);
            if (!_servicesTypes.ContainsKey(contract))
                throw new ApplicationException(string.Format("Запрошенного сервиса {0} не существует.", contract));

            if (_services.ContainsKey(contract))
            {
                return (T)_services[contract];
            }
            else
            {
                var serviceType = _servicesTypes[contract];
                var service = Activator.CreateInstance(serviceType);
                _services.Add(contract, service);

                return (T)service;
            }
        }
    }
}
