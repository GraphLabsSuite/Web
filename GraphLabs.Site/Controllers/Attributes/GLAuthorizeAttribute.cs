using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Controllers.Attributes
{
    /// <summary> Фильтр авторизации (принимает роли, для которых открыт доступ, через запятую) </summary>
    public class GLAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary> Фильтр авторизации </summary>
        public GLAuthorizeAttribute()
        {
        }

        /// <summary> Фильтр авторизации (принимает роли, для которых открыта доступ, через запятую) </summary>
        public GLAuthorizeAttribute(params UserRole[] allowedRoles)
        {
            Roles = string.Join(",", allowedRoles.Select(r => r.ToString()));
        }

        /// <summary> Обрабатывает HTTP-запрос, не прошедший авторизацию. </summary>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            Contract.Requires<ArgumentNullException>(filterContext != null);

            var httpContext = filterContext.HttpContext;
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var errorController = new ErrorController();
                filterContext.Result = errorController.InvokeHttp404(filterContext.HttpContext);
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}