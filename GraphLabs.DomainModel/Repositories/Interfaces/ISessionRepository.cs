using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using GraphLabs.Site.Utils;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel.EF.Repositories
{
    /// <summary> Репозиторий с сессиями </summary>
    [ContractClass(typeof(SessionRepositoryContracts))]
    public interface ISessionRepository
    {
        /// <summary> Найти сессию по идентификатору </summary>
        [CanBeNull]
        Session FindByGuid(Guid guid);

        /// <summary> Найти сессии по пользователю </summary>
        [NotNull]
        Session[] FindByUser(User user);

        /// <summary> Создать сессию </summary>
        [NotNull]
        Session Create(User user, string ip);

        /// <summary> Удалить сессию </summary>
        void Remove(Session session);

        /// <summary> Удалить несколько сессий </summary>
        void RemoveRange(IEnumerable<Session> sessions);
    }

    /// <summary> Репозиторий с сессиями - контракты </summary>
    [ContractClassFor(typeof(ISessionRepository))]
    internal abstract class SessionRepositoryContracts : ISessionRepository
    {
        // ReSharper disable AssignNullToNotNullAttribute

        /// <summary> Найти сессию по идентификатору </summary>
        public Session FindByGuid(Guid guid)
        {
            return default(Session);
        }

        /// <summary> Найти сессии по пользователю </summary>
        public Session[] FindByUser(User user)
        {
            Contract.Requires(user != null);
            Contract.Ensures(Contract.Result<Session[]>() != null);

            return default(Session[]);
        }

        /// <summary> Создать сессию </summary>
        public Session Create(User user, string ip)
        {
            Contract.Requires(user != null);
            Contract.Requires(IpHelper.CheckIsValidIP(ip));

            Contract.Ensures(Contract.Result<Session>() != null);

            return default(Session);
        }

        /// <summary> Удалить сессию </summary>
        public void Remove(Session session)
        {
            Contract.Requires(session != null);
        }

        /// <summary> Удалить несколько сессий </summary>
        public void RemoveRange(IEnumerable<Session> sessions)
        {
            Contract.Requires(sessions != null);
        }
    }
}