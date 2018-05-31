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
            Guard.IsNotNull(nameof(principal), principal);
            Guard.IsTrueAssertion(principal is IGraphLabsPrincipal);

            return ((IGraphLabsPrincipal)principal).IsInRole(role);
        }

        /// <summary> Есть ли у пользователя какая-либо из ролей? </summary>
        public static bool IsInAnyRole(this IPrincipal principal, params UserRole[] roles)
        {
            Guard.IsNotNull(nameof(principal), principal);
            Guard.IsTrueAssertion(principal is IGraphLabsPrincipal);
            Guard.IsTrueAssertion(roles != null && roles.Any());

            return roles.Any(role => ((IGraphLabsPrincipal)principal).IsInRole(role));
        }

        /// <summary> Отображаемое имя </summary>
        public static string DisplayName(this IIdentity identity)
        {
            Guard.IsNotNull(nameof(identity), identity);
            Guard.IsTrueAssertion(identity is IGraphLabsPrincipal);

            return ((IGraphLabsIdentity)identity).DisplayName;
        }
    }
}