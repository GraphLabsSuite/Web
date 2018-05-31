using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.Site.Utils;

namespace GraphLabs.Dal.Ef.Repositories
{
    /// <summary> Репозиторий с сессиями </summary>
    internal class SessionRepository : RepositoryBase, ISessionRepository
    {
        private readonly ISystemDateService _systemDateService;

        /// <summary> Репозиторий с сессиями </summary>
        public SessionRepository(GraphLabsContext context, ISystemDateService systemDateService)
            : base(context)
        {
            _systemDateService = systemDateService;
        }

        /// <summary> Найти сессию по идентификатору (подгружает вместе с пользователем) </summary>
        public Session FindByGuid(Guid guid)
        {
            CheckNotDisposed();

            return Context.Sessions
                .Where(s => s.Guid == guid)
                .Include(s => s.User)
                .SingleOrDefault();
        }

        /// <summary> Найти сессии по пользователю </summary>
        public Session[] FindByUser(User user)
        {
            Guard.IsNotNull(user);
            CheckNotDisposed();
            var result = Context.Sessions.Where(s => s.User.Id == user.Id).ToArray();
            Guard.IsNotNull(result,);
            return result;
        }

        /// <summary> Создать сессию </summary>
        public Session Create(User user, string ip)
        {
            Guard.IsNotNull(nameof(user), user);
            Guard.IsTrueAssertion(IpHelper.CheckIsValidIP(ip));
            CheckNotDisposed();

            var now = _systemDateService.Now();
            var session = Context.Sessions.Create();
            session.Guid = Guid.NewGuid();
            session.User = user;
            session.IP = ip;
            session.CreationTime = now;
            session.LastAction = now;
            Context.Sessions.Add(session);
            Guard.IsNotNull(nameof(session), session);
            
            return session;
        }

        /// <summary> Удалить сессию </summary>
        public void Remove(Session session)
        {
            Guard.IsNotNull(nameof(session), session);
            CheckNotDisposed();

            Context.Sessions.Remove(session);
        }

        /// <summary> Удалить несколько сессий </summary>
        public void RemoveRange(IEnumerable<Session> sessions)
        {
            Guard.IsNotNull(nameof(sessions), sessions);
            CheckNotDisposed();

            Context.Sessions.RemoveRange(sessions);
        }
    }
}
