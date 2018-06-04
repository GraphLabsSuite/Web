using System;
using System.Diagnostics.Contracts;
using System.Web;
using System.Web.Security;
using GraphLabs.Dal.Ef.Services;
using JetBrains.Annotations;
using log4net.Util;

namespace GraphLabs.Site
{
    /// <summary> Сервис фиксации результата аутентификации </summary>
    [UsedImplicitly]
    internal class FormsAuthenticationSavingService : IAuthenticationSavingService
    {
        private readonly ISystemDateService _systemDateService;
        private const string AUTH_COOKIE_NAME = "__ticket";
        private const int CURRENT_VERSION = 1;

        public FormsAuthenticationSavingService(ISystemDateService systemDateService)
        {
            _systemDateService = systemDateService;
        }

        /// <summary> Ответ </summary>
        private HttpResponse Response
        {
            get { return HttpContext.Current.Response; }
        }

        /// <summary> Запрос </summary>
        private HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        /// <summary> Пишем в cookies, что вошли </summary>
        public void SignIn(string email, Guid sessionGuid)
        {
            if (!Request.Browser.Cookies)
                throw new NotSupportedException("Браузер должен поддерживать cookies.");

            var now = _systemDateService.Now();
            var expiration = now.Add(FormsAuthentication.Timeout);
            var authTicket = new FormsAuthenticationTicket(
                CURRENT_VERSION,
                email,
                now,
                expiration,
                false,
                sessionGuid.ToString());
            var encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            var cookie = new HttpCookie(AUTH_COOKIE_NAME, encryptedTicket);
            Response.SetCookie(cookie);
        }

        private readonly HttpCookie _emptyAuthCookie = new HttpCookie(AUTH_COOKIE_NAME, null);

        /// <summary> Пишем в cookies, что вышли </summary>
        public void SignOut()
        {
            Response.SetCookie(_emptyAuthCookie);
        }

        /// <summary> Получить текущую сессию из cookies </summary>
        public ISessionInfo GetSessionInfo()
        {
            Guid sessionGuid;
            var ticket = FindAuthTicket();
            if (ticket != null && Guid.TryParse(ticket.UserData, out sessionGuid))
            {
                return new SessionInfo(ticket.Name, sessionGuid);
            }
            
            return SessionInfo.Empty;
        }

        [CanBeNull]
        private FormsAuthenticationTicket FindAuthTicket()
        {
            var encryptedTicket = Request.Cookies[AUTH_COOKIE_NAME];
            if (encryptedTicket == null || string.IsNullOrEmpty(encryptedTicket.Value)) 
            {
                return null;
            }

            FormsAuthenticationTicket ticket = null;
            try
            {
                ticket = FormsAuthentication.Decrypt(encryptedTicket.Value);
            }
            catch (ArgumentException)
            {
            }

            if (ticket != null && TicketIsValid(ticket))
                return ticket;

            return null;
        }

        private bool TicketIsValid(FormsAuthenticationTicket ticket)
        {
            return (ticket.IsPersistent || !ticket.Expired) && ticket.Version == CURRENT_VERSION;
        }

        /// <summary> Информация о сессии </summary>
        private class SessionInfo : ISessionInfo
        {
            /// <summary> Email </summary>
            public string Email { get; private set; }

            /// <summary> Guid сессии </summary>
            public Guid SessionGuid { get; private set; }

            /// <summary> Пустая информация? </summary>
            public bool IsEmpty()
            {
                return Email == null || SessionGuid == Guid.Empty;
            }

            private SessionInfo()
            {
            }

            /// <summary> Информация о сессии </summary>
            public SessionInfo(string email, Guid guid)
            {
                Guard.IsNotNullOrWhiteSpace(email);
                Guard.IsTrueAssertion(guid != Guid.Empty, "Сессия не должна быть null." );

                Email = email;
                SessionGuid = guid;
            }

            /// <summary> Пустая информация </summary>
            public static readonly SessionInfo Empty = new SessionInfo { Email = null, SessionGuid = Guid.Empty };
        }
    }
}