using System;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;

namespace GraphLabs.Site
{
    /// <summary> Сервис фиксации результата аутентификации </summary>
    [ContractClass(typeof(AuthenticationSavingServiceContracts))]
    public interface IAuthenticationSavingService
    {
        /// <summary> Пишем в cookies, что вошли </summary>
        void SignIn(string email, Guid sessionGuid);

        /// <summary> Пишем в cookies, что вышли </summary>
        void SignOut();

        /// <summary> Получить текущую сессию из cookies </summary>
        [NotNull]
        ISessionInfo GetSessionInfo();
    }

    /// <summary> Информация о сессии </summary>
    public interface ISessionInfo
    {
        /// <summary> Email </summary>
        string Email { get; }

        /// <summary> Guid сессии </summary>
        Guid SessionGuid { get; }

        /// <summary> Пустая информация? </summary>
        bool IsEmpty();
    }

    /// <summary> Сервис фиксации результата аутентификации - контракты </summary>
    [ContractClassFor(typeof(IAuthenticationSavingService))]
    internal abstract class AuthenticationSavingServiceContracts : IAuthenticationSavingService
    {
        // ReSharper disable AssignNullToNotNullAttribute

        /// <summary> Пишем в cookies, что вошли </summary>
        public void SignIn(string email, Guid sessionGuid)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(email));
            Contract.Requires(sessionGuid != Guid.Empty);
        }

        /// <summary> Пишем в cookies, что вышли </summary>
        public void SignOut()
        {
        }

        /// <summary> Получить текущую сессию из cookies </summary>
        public ISessionInfo GetSessionInfo()
        {
            return default(ISessionInfo);
        }

    }
}