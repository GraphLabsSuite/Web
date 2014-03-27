using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Controllers
{
    /// <summary> Фильтер авторизации </summary>
    public class GLAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary> Фильтер авторизации </summary>
        public GLAuthorizeAttribute()
        {
        }

        /// <summary> Фильтер авторизации </summary>
        public GLAuthorizeAttribute(UserRole roles)
        {
            var activeRoles = Enum
                .GetValues(typeof(UserRole))
                .Cast<UserRole>()
                .Where(role => roles.HasFlag(role))
                .Select(role => role.ToString());

            Roles = string.Join(",", activeRoles);
        }
    }
}