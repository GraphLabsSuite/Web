using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;

namespace GraphLabs.Site.Utils
{
    /// <summary> Расширения безопасности для контроллера </summary>
    public static class SecurityExtensions
    {
        public const int MIN_PASSWORD_LENGTH = 6;

        private const string SESSION_GUID = "key";

        #region Log In/Out

        /// <summary> Выполняет вход </summary>
        public static bool Login(this Controller controller, GraphLabsContext ctx, string login, string password)
        {
            var foundUser = (from u in ctx.Users
                             where u.Login == login && (!(u is Student) || (u as Student).IsVerified)
                             select u
                             ).SingleOrDefault();
            if (foundUser == null)
                return false;

            var passHash = HashCalculator.GenerateSaltedHash(password, foundUser.HashSalt);

            if (!passHash.SequenceEqual(foundUser.PasswordHash))
                return false;

            controller.FillSession(ctx, foundUser);

            return true;
        }

        private static void FillSession(this Controller controller, GraphLabsContext ctx, User user)
        {
            var guid = Guid.NewGuid();

            controller.Session[SESSION_GUID] = guid.ToString();

            // Если остались активные сессии - убьём их
            var oldSessions = ctx.Sessions.Where(s => s.User.Id == user.Id);
            foreach (var s in oldSessions)
            {
                //TODO: если s.IsActive() спросить у пользователя, хочет ли он убить другую сессию
                ctx.Sessions.Remove(s);
            }

            // Впишем новую
            var newSession = new Session
            {
                CreationTime = DateTime.Now,
                LastAction = DateTime.Now,
                User = user,
                Guid = guid,
                CheckSumm = controller.Session.CalculateCheckSumm(user),
                IP = controller.GetClientIP()
            };
            ctx.Sessions.Add(newSession);
            ctx.SaveChanges();
        }

        /// <summary> Выход </summary>
        public static void Logout(this Controller controller, GraphLabsContext ctx)
        {
            var guid = new Guid((string)controller.Session[SESSION_GUID]);

            var session = ctx.Sessions.SingleOrDefault(s => s.Guid == guid);
            if (session != null)
            {
                ctx.Sessions.Remove(session);
                ctx.SaveChanges();
            }

            controller.Session.Remove(SESSION_GUID);
        }

        #endregion // Log In/Out

        /// <summary> Проверяет, авторизован ли пользователь, но не выполняет никаких проверок сессии </summary>
        public static bool IsAuthenticated(this Controller controller)
        {
            return !string.IsNullOrWhiteSpace((string)controller.Session[SESSION_GUID]);
        }

        /// <summary> Проверяет, что пользователь авторизован и сессия правильная; переставляет время последнего действия; заполняет ViewBag. </summary>
        public static bool CheckAuthentication(this Controller controller, GraphLabsContext ctx)
        {
            // Проверим, что вообще залогинены
            if (!controller.IsAuthenticated())
                return false;

            // Попытаемся вытащить нужную сессию по её guid и id пользователя
            var session = controller.Session.GetFromDb(ctx);
            if (session == null)
                return false;

            if (controller.GetClientIP() != session.IP)
                return false;

            // Проверим контрольную сумму
            if (session.CheckSumm != controller.Session.CalculateCheckSumm(session.User))
                return false;

            // Всё ОК. Отметим последнее действие
            session.LastAction = DateTime.Now;
            ctx.SaveChanges();

            // Заполним ViewBag
            controller.ViewBag.IsAuthenticated = true;
            controller.ViewBag.UserName = controller.Session.GetUser(ctx).GetShortName();

            return true;
        }

        /// <summary> Возвращает текущего пользователя по сессии </summary>
        public static User GetUser(this HttpSessionStateBase session, GraphLabsContext ctx)
        {
            var dbSession = session.GetFromDb(ctx);
            return dbSession.User;
        }

        /// <summary> Получает IP клиента </summary>
        public static string GetClientIP(this Controller controller)
        {
            return controller.Request.ServerVariables["REMOTE_ADDR"];
        }

        /// <summary> Проверяет, имеет ли пользователь указанную роль, + проверка сессии </summary>
        public static bool CheckRole(this Controller controller, GraphLabsContext ctx, UserRole role)
        {
            if (!CheckAuthentication(controller, ctx))
                return false;
            var user = controller.Session.GetUser(ctx);
            
            return CheckRole(user, role);
        }

        #region Вспомогательное

        private static Session GetFromDb(this HttpSessionStateBase session, GraphLabsContext ctx)
        {
            var guid = new Guid(session.GetGuid());
            return ctx.Sessions.Single(s => s.Guid == guid);
        }

        private static string GetGuid(this HttpSessionStateBase session)
        {
            return (string)session[SESSION_GUID];
        }

        /// <summary> Считает контрольную сумму сессии </summary>
        /// <remarks> 
        /// Контрольная сумма - функция от трёх артументов: 
        ///  - генерируемого нами guid сессии (ездит к клиенту), 
        ///  - генерируемого asp.net'ом SessionID (судя по всему, тоже ездит к клиенту)
        ///  - id пользователя, который теперь НЕ ездит к клиенту - нужно следить, чтобы так были и впредь
        /// </remarks>
        private static string CalculateCheckSumm(this HttpSessionStateBase session, User user)
        {
            var guid = session.GetGuid();
            var saltBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Concat(session.SessionID, user.Id)));

            return HashCalculator.GenerateSaltedHash(guid, saltBase64);
        }

        private static bool CheckRole(User user, UserRole role)
        {
            switch (role)
            {
                case UserRole.Student:
                    return true;
                case UserRole.Teacher:
                    return user.Role != UserRole.Student;
                case UserRole.Administrator:
                    return user.Role == UserRole.Administrator;
                default:
                    throw new NotSupportedException("Что-то тут забыли.");
            }
        }

        #endregion


    }
}