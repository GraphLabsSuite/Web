using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Principal;
using GraphLabs.DomainModel;
using GraphLabs.Dal.Ef;

namespace GraphLabs.Site.Logic.Security
{
    /// <summary> Всякие полезные расширения </summary>
    public static class SecurityExtensions
    {
        /// <summary> Есть ли у пользователя роль? </summary>
        public static bool IsInRole(this IPrincipal principal, UserRole role)
        {
            Contract.Requires<ArgumentNullException>(principal != null);
            Contract.Requires<ArgumentException>(principal is IGraphLabsPrincipal);

            return ((IGraphLabsPrincipal)principal).IsInRole(role);
        }

        /// <summary> Есть ли у пользователя какая-либо из ролей? </summary>
        public static bool IsInAnyRole(this IPrincipal principal, params UserRole[] roles)
        {
            Contract.Requires<ArgumentNullException>(principal != null);
            Contract.Requires<ArgumentException>(principal is IGraphLabsPrincipal);
            Contract.Requires<ArgumentException>(roles != null && roles.Any());

            return roles.Any(role => ((IGraphLabsPrincipal)principal).IsInRole(role));
        }

        /// <summary> Отображаемое имя </summary>
        public static string DisplayName(this IIdentity identity)
        {
            Contract.Requires<ArgumentNullException>(identity != null);
            Contract.Requires<ArgumentException>(identity is IGraphLabsIdentity);

            return ((IGraphLabsIdentity)identity).DisplayName;
        }
    }
}