using System;
using System.Diagnostics.Contracts;
using System.Security.Principal;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Logic.Security
{
    /// <summary> Пользовательские роли </summary>
    public static class PrincipalExtensions
    {
        /// <summary> Есть ли у пользователя роль? </summary>
        public static bool IsInRole(this IPrincipal principal, UserRole role)
        {
            Contract.Requires<ArgumentNullException>(principal != null);
            Contract.Requires<ArgumentException>(principal is GraphLabsPrincipal);

            return ((GraphLabsPrincipal)principal).IsInRole(role);
        }

    }
}