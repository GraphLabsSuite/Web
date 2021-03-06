﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel.Repositories;

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
            CheckNotDisposed();

            return Context.Sessions.Where(s => s.User.Id == user.Id).ToArray();
        }

        /// <summary> Создать сессию </summary>
        public Session Create(User user, string ip)
        {
            CheckNotDisposed();

            var now = _systemDateService.Now();
            var session = Context.Sessions.Create();
            session.Guid = Guid.NewGuid();
            session.User = user;
            session.IP = ip;
            session.CreationTime = now;
            session.LastAction = now;
            Context.Sessions.Add(session);

            return session;
        }

        /// <summary> Удалить сессию </summary>
        public void Remove(Session session)
        {
            CheckNotDisposed();

            Context.Sessions.Remove(session);
        }

        /// <summary> Удалить несколько сессий </summary>
        public void RemoveRange(IEnumerable<Session> sessions)
        {
            CheckNotDisposed();

            Context.Sessions.RemoveRange(sessions);
        }
    }
}
