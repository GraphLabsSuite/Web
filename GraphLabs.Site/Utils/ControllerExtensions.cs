using System;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Utils
{
    /// <summary> Класс с расширениями для контроллера </summary>
    public static class ControllerExtensions
    {
        /// <summary> Получает IP клиента </summary>
        public static string GetClientIP(this Controller controller)
        {
            return controller.Request.ServerVariables["REMOTE_ADDR"];
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
        
        /// <summary> Проверяет, имеет ли пользователь указанную роль, + проверки сессии </summary>
        public static bool CheckUser(this Controller controller, UserRole role)
        {
            var idUser = (long)controller.Session["id_user"];
            var guidString = (string)controller.Session["guid"];

            if (string.IsNullOrWhiteSpace(guidString))
                return false;

            using (var ctx = new GraphLabsContext())
            {
                var user = ctx.Users.Find(idUser);
                if (user == null)
                    return false;
                if (!CheckRole(user, role))
                    return false;
                var guid = new Guid(guidString);
                return ctx.Sessions.SingleOrDefault(s => s.Guid == guid && s.User == user && s.IsActive) != null;
            }
        }
    }
}