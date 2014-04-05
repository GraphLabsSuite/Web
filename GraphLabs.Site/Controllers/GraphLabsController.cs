using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GraphLabs.Site.Logic.Security;

namespace GraphLabs.Site.Controllers
{
    /// <summary> Базовый контроллер </summary>
    [HandleError]
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
                    String.Format("Для действия {0} не указан способ авторизации.", filterContext.ActionDescriptor.ActionName));
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


        #region Errors

        /// <summary> Вызывается, когда в действии происходит необработанное исключение. </summary>
        /// <param name="filterContext">Сведения о текущем запросе и действии.</param>
        protected override void OnException(ExceptionContext filterContext)
        {
            var errorController = new ErrorController();

            var errorRoute = new RouteData();
            errorRoute.Values.Add("controller", "Error");
            errorRoute.Values.Add("action", "Oops");
            errorRoute.Values.Add("exception", filterContext.Exception);
            errorController.Execute(new RequestContext(filterContext.HttpContext, errorRoute));

            filterContext.ExceptionHandled = true;
        }

        /// <summary> Наша обработка NotFound </summary>
        protected override HttpNotFoundResult HttpNotFound(string statusDescription)
        {
            InvokeHttp404(HttpContext);
            return base.HttpNotFound(statusDescription);
        }

        /// <summary> Вызывается, когда запрос соответствует этому контроллеру, но в контроллере не найден метод с указанным именем действия. </summary>
        /// <param name="actionName">Имя действия, которое требовалось выполнить.</param>
        protected override void HandleUnknownAction(string actionName)
        {
            // Если контроллер - ErrorController, то не нужно снова вызывать исключение
            if (GetType() != typeof(ErrorController))
            {
                InvokeHttp404(HttpContext);
            }
        }


        /// <summary> 404 </summary>
        public ActionResult InvokeHttp404(HttpContextBase httpContext)
        {
            var errorController = new ErrorController();

            var errorRoute = new RouteData();
            errorRoute.Values.Add("controller", "Error");
            errorRoute.Values.Add("action", "Error404");
            errorRoute.Values.Add("url", httpContext.Request.Url != null ? httpContext.Request.Url.OriginalString : String.Empty);
            errorController.Execute(new RequestContext(httpContext, errorRoute));

            return new EmptyResult();
        }

        #endregion

        /// <summary> Ключ стандартного сообщения валидации </summary>
        public const string STD_VALIDATION_MSG_KEY = "validation";
    }
}