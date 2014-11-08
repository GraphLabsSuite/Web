using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.Site.Logic.Security;

namespace GraphLabs.Site.Utils
{
    /// <summary> Расширения безопасности для контроллера </summary>
    public static class SecurityExtensions
    {
        public const int MIN_PASSWORD_LENGTH = 6;

        private const string REMOTE_ADDR = "REMOTE_ADDR";


        /// <summary> Получить IP клиента </summary>
        public static string GetClientIP(this HttpRequestBase request)
        {
            return request.ServerVariables[REMOTE_ADDR];
        }

        /// <summary> Получить IP клиента </summary>
        public static string GetClientIP(this HttpRequest request)
        {
            return request.ServerVariables[REMOTE_ADDR];
        }
    }
}