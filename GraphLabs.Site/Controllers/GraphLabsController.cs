using System;
using System.Web.Mvc;
using GraphLabs.Site.Logic.Security;

namespace GraphLabs.Site.Controllers
{
    /// <summary> Базовый контроллер </summary>
    public abstract class GraphLabsController : Controller
    {
        /// <summary> Текущий пользователь </summary>
        protected new IGraphLabsPrincipal User
        {
            get
            {
                return (IGraphLabsPrincipal)base.User;
            }
        }

        /// <summary> IOC-контейнер </summary>
        protected IDependencyResolver DependencyResolver
        {
            get { return System.Web.Mvc.DependencyResolver.Current; }
        }

        /// <summary> Вызывается при выполнении авторизации. </summary>
        /// <param name="filterContext">Сведения о текущем запросе и действии.</param>
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            var allowAnonymous =
                filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);

            var hasAuthorize = filterContext.ActionDescriptor.IsDefined(typeof(GLAuthorizeAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(GLAuthorizeAttribute), true);
            
            if (!allowAnonymous && !hasAuthorize)
            {
#if DEBUG
                throw new InvalidOperationException(
                    string.Format("Для действия {0} не указан способ авторизации.", filterContext.ActionDescriptor.ActionName));
#else
                filterContext.Result = RedirectToAction("index", "home");
#endif
            }

        }

        #region Helpers

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion
    }
}