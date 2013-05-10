using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
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
        private const string REMOTE_ADDR = "REMOTE_ADDR";


        #region Log In/Out

        /// <summary> Выполняет вход </summary>
        public static bool Login(this Controller controller, GraphLabsContext ctx, string email, string password)
        {
            var foundUser = (from u in ctx.Users
                             where u.Email == email && (!(u is Student) || (u as Student).IsVerified)
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
            var sessionGUID = (string)controller.Session[SESSION_GUID];

            if (string.IsNullOrWhiteSpace(sessionGUID)) return;

            var guid = new Guid(sessionGUID);

            var session = ctx.Sessions.SingleOrDefault(s => s.Guid == guid);
            if (session != null)
            {
                ctx.Sessions.Remove(session);
                ctx.SaveChanges();
            }

            controller.Session.Remove(SESSION_GUID);
        }

        #endregion // Log In/Out


        #region Проверки во View

        /// <summary> Проверяет, что для страницы указан режим доступа </summary>
        public static string CheckAuthentication(this WebViewPage page)
        {
            #if DEBUG
            var allowAnonymous = (bool?)page.ViewBag.AllowAnonymous;
            var isAuthenticated = (bool?)page.ViewBag.IsAuthenticated;
            
            Contract.Assume(allowAnonymous.HasValue || isAuthenticated.HasValue, "Для действия не указаны разрешения безопасности для страницы.");
            #endif
            
            return string.Empty;
        }

        /// <summary> Проверяет, что пользователь авторизован </summary>
        public static bool IsAuthenticated(this WebViewPage page)
        {
            var isAuthenticated = (bool?)page.ViewBag.IsAuthenticated;

            return isAuthenticated == true;
        }


        /// <summary> Обладает ли пользователь ролью? (например, администратор обладает ролями студента и преподавателя) </summary>
        public static bool IsUserInRole(this WebViewPage page, UserRole role)
        {
            if (!page.IsAuthenticated())
                return false;

            return CheckRole((UserRole)page.ViewBag.UserRole, role);
        }

        #endregion


        #region Провеврки в контроллере

        /// <summary> Помечает, что страница доступна к просмотру неавторизованному пользователю. </summary>
        public static void AllowAnonymous(this Controller controller, GraphLabsContext ctx)
        {
            if (controller.IsAuthenticated(ctx))
                return;

            controller.ViewBag.AllowAnonymous = true;
        }

        /// <summary> Проверяет, что пользователь авторизован и сессия правильная; переставляет время последнего действия; заполняет ViewBag. </summary>
        public static bool IsAuthenticated(this Controller controller, GraphLabsContext ctx)
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
            controller.FillViewBag(ctx);

            return true;
        }

        /// <summary> Проверяет, имеет ли пользователь указанную роль, + проверка сессии; прописывает роль во ViewBag </summary>
        /// <remarks> Например, администратор обладает ролями студента и преподавателя. </remarks>
        public static bool IsUserInRole(this Controller controller, GraphLabsContext ctx, UserRole role)
        {
            if (!IsAuthenticated(controller, ctx))
                return false;
            var user = controller.Session.GetUser(ctx);

            return CheckRole(user.Role, role);
        }

        #endregion

        /// <summary> Возвращает текущего пользователя по сессии </summary>
        public static User GetUser(this HttpSessionStateBase session, GraphLabsContext ctx)
        {
            var dbSession = session.GetFromDb(ctx);
            return dbSession.User;
        }

        /// <summary> Получает IP клиента </summary>
        public static string GetClientIP(this Controller controller)
        {
            return controller.Request.ServerVariables[REMOTE_ADDR];
        }


        #region Вспомогательное

        private static void FillViewBag(this Controller controller, GraphLabsContext ctx)
        {
            controller.ViewBag.IsAuthenticated = true;
            var user = controller.Session.GetUser(ctx);
            controller.ViewBag.UserName = user.GetShortName();
            controller.ViewBag.UserRole = user.Role;
        }

        /// <summary> Проверяет, авторизован ли пользователь, но не выполняет никаких проверок сессии </summary>
        private static bool IsAuthenticated(this Controller controller)
        {
            return !string.IsNullOrWhiteSpace((string)controller.Session[SESSION_GUID]);
        }

        private static Session GetFromDb(this HttpSessionStateBase session, GraphLabsContext ctx)
        {
            var guid = new Guid(session.GetGuid());
            return ctx.Sessions.SingleOrDefault(s => s.Guid == guid);
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

        private static bool CheckRole(UserRole availableRole, UserRole requiredRole)
        {
            switch (requiredRole)
            {
                case UserRole.Student:
                    return true;
                case UserRole.Teacher:
                    return availableRole != UserRole.Student;
                case UserRole.Administrator:
                    return availableRole == UserRole.Administrator;
                default:
                    throw new NotSupportedException("Что-то тут забыли.");
            }
        }

        #endregion


    }
}