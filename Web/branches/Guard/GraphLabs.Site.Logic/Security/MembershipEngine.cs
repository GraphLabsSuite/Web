using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using GraphLabs.DomainModel;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.Site.Core.OperationContext;
using log4net;
using GraphLabs.Site.Utils;

namespace GraphLabs.Site.Logic.Security
{
    // TODO разделить на часть, отвечающую за непосредственно создание новых пользователей и редактирование старых и на часть, отвечающую за аутентификацию
    /// <summary> Работа с пользователями </summary>
    public sealed class MembershipEngine : IMembershipEngine
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(MembershipEngine));

        private readonly IHashCalculator _hashCalculator;
        private readonly ISystemDateService _systemDateService;
        private readonly IOperationContextFactory<IGraphLabsContext> _operationFactory;

        /// <summary> Проверяет личность пользователя и тп </summary>
        public MembershipEngine(
            IHashCalculator hashCalculator, 
            ISystemDateService systemDateService,
            IOperationContextFactory<IGraphLabsContext> operationFactory) 
        {
            Contract.Requires(hashCalculator != null);
            Guard.Guard.IsNotNull(hashCalculator, nameof(hashCalculator));
            Contract.Requires(systemDateService != null);
            Guard.Guard.IsNotNull(systemDateService, nameof(systemDateService));

            _hashCalculator = hashCalculator;
            _systemDateService = systemDateService;
            _operationFactory = operationFactory;
        }

        /// <summary> Выполняет вход </summary>
        public LoginResult TryLogin(string email, string password, string clientIp, ref Guid sessionGuid)//ref, мы же не инициализируем его в функции 
        {
            Guard.Guard.IsTrueAssertion(IpHelper.CheckIsValidIP(clientIp));
            Guard.Guard.IsTrueAssertion(TryLoginImpl(email, password, clientIp, ref sessionGuid, force: false) != LoginResult.Success & sessionGuid == Guid.Empty ||
                TryLoginImpl(email, password, clientIp, ref sessionGuid, force: false) == LoginResult.Success & sessionGuid != Guid.Empty);
            return TryLoginImpl(email, password, clientIp, ref sessionGuid, force: false);
        }

        /// <summary> Выполняет вход, убивая все сессии на других браузерах/компьютерах </summary>
        public LoginResult TryForceLogin(string email, string password, string clientIp, ref Guid sessionGuid)
        {
            Guard.Guard.IsTrueAssertion(IpHelper.CheckIsValidIP(clientIp));
            Guard.Guard.IsTrueAssertion(TryLoginImpl(email, password, clientIp, ref sessionGuid, force: true) != LoginResult.Success & sessionGuid == Guid.Empty ||
                TryLoginImpl(email, password, clientIp, ref sessionGuid, force: true) == LoginResult.Success & sessionGuid != Guid.Empty);
            return TryLoginImpl(email, password, clientIp,  ref sessionGuid, force: true);
        }

        private LoginResult TryLoginImpl(string email, string password, string clientIp, ref Guid sessionGuid, bool force)
        {
            using (var operation = _operationFactory.Create())
            {
                var user = operation.QueryOf<User>()
                    .SingleOrDefault(u => u.Email == email &&
                                          (!(u is Student) || ((u as Student).IsVerified && !(u as Student).IsDismissed)));

                if (user == null || !UserIsValid(user, password))
                {
                    _log.InfoFormat("Неудачный вход, e-mail: {0}, ip: {1}", email, clientIp);

                    sessionGuid = Guid.Empty;
                    return LoginResult.InvalidLoginPassword;
                }

                var lastSessions = GetSessionsOrderedByCreationTime(operation.DataContext, user);
                if (lastSessions.Any())
                {
                    if (!force)
                    {
                        sessionGuid = Guid.Empty;
                        return LoginResult.LoggedInWithAnotherSessionId;
                    }

                    foreach (var s in lastSessions)
                    {
                        operation.DataContext.Factory.Delete(s);
                    }
                }

                var session = CreateSession(operation.DataContext.Factory, user, clientIp);
                SetLastAction(session);

                _log.InfoFormat("Удачный вход, e-mail: {0}, ip: {1}", email, clientIp);
                SetupCurrentPrincipal(user);

                sessionGuid = session.Guid;

                operation.Complete();
            }

            return LoginResult.Success;
        }

        private Session CreateSession(IEntityFactory factory, User user, string ip)
        {
            var now = _systemDateService.Now();
            var session = factory.Create<Session>();

            session.Guid = Guid.NewGuid();
            session.User = user;
            session.IP = ip;
            session.CreationTime = now;
            session.LastAction = now;

            return session;
        }

        /// <summary> Выход </summary>
        public void Logout(string email, Guid sessionGuid, string clientIp)
        {
            using (var operation = _operationFactory.Create())
            {
                var session = FindSession(operation.DataContext.Query, email, sessionGuid, clientIp);
                if (session == null)
                {
                    _log.InfoFormat("Неудачная попытка выхода - сессия не найдена. email: {0}, guid: {1}, ip: {2}",
                        email, sessionGuid, clientIp);
                    SetupCurrentPrincipal(null);
                    return;
                }

                operation.DataContext.Factory.Delete(session);
                SetupCurrentPrincipal(null);

                operation.Complete();
                _log.InfoFormat("Успешный выход. e-mail: {0}, ip: {1}", email, clientIp);
            }
        }

        /// <summary> Проверяем пользователя и устанавливаем IPrincipal </summary>
        public bool TryAuthenticate(string email, Guid sessionGuid, string clientIp)
        {
            if (string.IsNullOrWhiteSpace(email) && sessionGuid == Guid.Empty)
            {
                SetupCurrentPrincipal(null);
                return false;
            }

            using (var operation = _operationFactory.Create())
            {
                var session = FindSession(operation.DataContext.Query, email, sessionGuid, clientIp);
                if (session == null)
                {
                    _log.InfoFormat(
                        "Неудачная проверка пользователя - сессия не найдена или некорректна. email: {0}, guid: {1}, ip: {2}",
                        email, sessionGuid, clientIp);
                    SetupCurrentPrincipal(null);
                    return false;
                }

                SetLastAction(session);
                SetupCurrentPrincipal(session.User);

                operation.Complete();
            }

            return true;
        }

        /// <summary> Поменять пароль </summary>
        public bool ChangePassword(string email, Guid sessionGuid, string clientIp, string currentPassword, string newPassword)
        {
            Guard.Guard.IsNotNullOrWhiteSpace(email);
            Guard.Guard.IsTrueAssertion(sessionGuid != Guid.Empty);
            Guard.Guard.IsTrueAssertion(IpHelper.CheckIsValidIP(clientIp));
            Guard.Guard.IsNotNullOrWhiteSpace(currentPassword);
            Guard.Guard.IsNotNullOrWhiteSpace(newPassword);

            using (var operation = _operationFactory.Create())
            {
                var session = FindSession(operation.DataContext.Query, email, sessionGuid, clientIp);
                if (session == null)
                {
                    _log.WarnFormat(
                        "Неудачная попытка смены пароля - сессия не найдена. email: {0}, guid: {1}, ip: {2}", email,
                        sessionGuid, clientIp);
                    return false;
                }

                var user = session.User;
                if (!UserIsValid(user, currentPassword))
                {
                    return false;
                }

                user.PasswordHash = _hashCalculator.Crypt(newPassword);
                operation.Complete();
            }

            return true;
        }

        /// <summary> Зарегистрировать нового студента</summary>
        /// <returns> false, если такой пользователь уже есть (с таким email), иначе true </returns>
        public bool RegisterNewStudent(string email, string name, string fatherName, string surname, string password, long groupId)
        {
            Guard.Guard.IsNotNullOrWhiteSpace(email);
            Guard.Guard.IsNotNullOrWhiteSpace(name);
            Guard.Guard.IsNotNullOrWhiteSpace(surname);
            Guard.Guard.IsNotNullOrWhiteSpace(password);
            Guard.Guard.IsTrueAssertion(groupId > 0);

            var passHash = _hashCalculator.Crypt(password);

            using (var operation = _operationFactory.Create())
            {
                var group = operation.DataContext.Query.Get<Group>(groupId);

                var student = operation.DataContext.Factory.Create<Student>();
                student.PasswordHash = passHash;
                student.Name = name;
                student.Surname = surname;
                student.FatherName = fatherName;
                student.Email = email;
                student.IsDismissed = false;
                student.IsVerified = false;
                student.Role = UserRole.Student;
                student.Group = group;

                try
                {
                    operation.Complete();
                }
                catch (DbUpdateException ex)
                {
                    _log.InfoFormat("Не удалось зарегистрировать студента. {0}", ex);
                    return false;
                }

                _log.InfoFormat("Студент зарегистрирован. email: {0}", email);
            }

            return true;
        }


        #region Установка IPrincipal

        private IPrincipal GetPrincipal(User user)
        {
            return user == null
                ? GraphLabsPrincipal.Anonymous
                : new GraphLabsPrincipal(user.Email, user.GetShortName(), user.Role);
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

        private Session FindSession(IEntityQuery query, string email, Guid sessionGuid, string clientIp)
        {
            var session = query
                .OfEntities<Session>()
                .Where(s => s.Guid == sessionGuid)
                .Include(s => s.User)
                .SingleOrDefault();

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
        private Session[] GetSessionsOrderedByCreationTime(IGraphLabsContext ctx, User user)
        {
            return ctx.Query
                .OfEntities<Session>()
                .Where(s => s.User.Id == user.Id)
                .OrderByDescending(s => s.CreationTime)
                .ToArray();
        }

        #endregion
    }
}
