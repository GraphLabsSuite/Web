using System;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.DomainModel.Services;
using log4net;

namespace GraphLabs.Site.Logic.Security
{
    /// <summary> Работа с пользователями </summary>
    public sealed class MembershipEngine : IMembershipEngine
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(MembershipEngine));

        private readonly TransactionManager _transactionManager;
        private readonly IHashCalculator _hashCalculator;
        private readonly ISystemDateService _systemDateService;
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ISessionRepository _sessionRepository;

        /// <summary> Проверяет личность пользователя и тп </summary>
        public MembershipEngine(
            TransactionManager transactionManager,
            IHashCalculator hashCalculator, 
            ISystemDateService systemDateService,
            IUserRepository userRepository,
            IGroupRepository groupRepository,
            ISessionRepository sessionRepository)
        {
            Contract.Requires(hashCalculator != null);
            Contract.Requires(systemDateService != null);

            _transactionManager = transactionManager;
            _hashCalculator = hashCalculator;
            _systemDateService = systemDateService;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _sessionRepository = sessionRepository;
        }

        /// <summary> Выполняет вход </summary>
        public bool TryLogin(string email, string password, string clientIp, out Guid sessionGuid)
        {
            var user = _userRepository.FindActiveUserByEmail(email);

            if (user == null || !UserIsValid(user, password))
            {
                _log.InfoFormat("Неудачный вход, e-mail: {0}, ip: {1}", email, clientIp);
                sessionGuid = Guid.Empty;
                return false;
            }

            Session session;
            var lastSession = RemoveOldSessionsExceptLast(user);
            if (lastSession == null || !SessionIsValid(lastSession, email, clientIp))
            {
                if (lastSession != null)
                    _sessionRepository.Remove(lastSession);
                session = _sessionRepository.Create(user, clientIp);
            }
            else
            {
                session = lastSession;
            }
            SetLastAction(session);
            _transactionManager.IntermediateCommit();
            _log.InfoFormat("Удачный вход, e-mail: {0}, ip: {1}", email, clientIp);

            SetupCurrentPrincipal(user);
            sessionGuid = session.Guid;
            return true;
        }

        /// <summary> Выход </summary>
        public void Logout(string email, Guid sessionGuid, string clientIp)
        {
            var session = FindSession(email, sessionGuid, clientIp);
            if (session == null)
            {
                _log.InfoFormat("Неудачная попытка выхода - сессия не найдена. email: {0}, guid: {1}, ip: {2}", email, sessionGuid, clientIp);
                SetupCurrentPrincipal(null);
                return;
            }

            _sessionRepository.Remove(session);
            _transactionManager.IntermediateCommit();
            _log.InfoFormat("Успешный выход. e-mail: {0}, ip: {1}", email, clientIp);
            SetupCurrentPrincipal(null);
        }

        /// <summary> Проверяем пользователя </summary>
        public bool TryAuthenticate(string email, Guid sessionGuid, string clientIp)
        {
            var session = FindSession(email, sessionGuid, clientIp);
            if (session == null)
            {
                _log.InfoFormat("Неудачная проверка пользователя - сессия не найдена или некорректна. email: {0}, guid: {1}, ip: {2}", email, sessionGuid, clientIp);
                SetupCurrentPrincipal(null);
                return false;
            }

            SetLastAction(session);
            _transactionManager.IntermediateCommit();

            SetupCurrentPrincipal(session.User);
            return true;
        }

        /// <summary> Зарегистрировать нового студента</summary>
        /// <returns> false, если такой пользователь уже есть (с таким email), иначе true </returns>
        public bool RegisterNewStudent(string email, string name, string fatherName, string surname, string password, long groupId)
        {
            var passHash = _hashCalculator.Crypt(password);

            var group = _groupRepository.GetGroupById(groupId);
            try
            {
                _userRepository.CreateNotVerifiedStudent(email, name, fatherName, surname, passHash, group);
            }
            catch (DbUpdateException ex)
            {
                _log.InfoFormat("Не удалось зарегистрировать студента. {0}", ex);
                return false;
            }
            
            _log.InfoFormat("Студент зарегистрирован. email: {0}", email);
            return true;
        }


        #region Установка IPrincipal

        private IPrincipal GetPrincipal(User user)
        {
            return user == null 
                ? GraphLabsPrincipal.Anonymous 
                : new GraphLabsPrincipal(user.GetShortName(), user.Email, user.Role);
        }

        private void SetupCurrentPrincipal(User user)
        {
            var principal = GetPrincipal(user);
            
            Thread.CurrentPrincipal = principal;
            
            var context = HttpContext.Current;
            if (context != null)
                context.User = principal;
        }

        #endregion


        #region Вспомогательное

        private Session FindSession(string email, Guid sessionGuid, string clientIp)
        {
            var session = _sessionRepository.FindByGuid(sessionGuid);
            if (session != null && SessionIsValid(session, email, clientIp))
            {
                return session;
            }
            return null;
        }

        private void SetLastAction(Session session)
        {
            session.LastAction = _systemDateService.Now();
        }

        private bool UserIsValid(User user, string password)
        {
            var student = user as Student;
            return (student == null || !student.IsDismissed && student.IsVerified) &&
                _hashCalculator.Verify(password, user.PasswordHash);
        }

        private bool SessionIsValid(Session session, string email, string ip)
        {
            return session.IP == ip && session.User.Email == email;
        }

        /// <summary> Удаляет все старые сессии, кроме последней</summary>
        /// <returns> Возвращает последнюю по времени сессию, если таковая вообще есть </returns>
        private Session RemoveOldSessionsExceptLast(User user)
        {
            var oldSessions = _sessionRepository.FindByUser(user).OrderByDescending(s => s.CreationTime);
            _sessionRepository.RemoveRange(oldSessions.Skip(1));

            return oldSessions.FirstOrDefault();
        }

        #endregion
    }
}
