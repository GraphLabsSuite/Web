using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GraphLabs.Site.App_Start;
using GraphLabs.Site.Logic.Security;
using GraphLabs.Site.ServicesConfig;
using GraphLabs.Site.Utils;
using Microsoft.Practices.Unity;

namespace GraphLabs.Site
{
    /// <summary> Приложение GraphLabs - точка входа </summary>
    public class GraphLabsApplication : HttpApplication
    {
        private static readonly string _requestUnitOfWork = "transactionManager";

        private static HttpContext CurrentContext => HttpContext.Current;

        private static RequestUnitOfWork GetRequestUnitOfWork()
        {
            if (CurrentContext == null)
                return null;

            var mgr = (RequestUnitOfWork)CurrentContext.Items[_requestUnitOfWork];
            if (mgr == null)
            {
                mgr = new RequestUnitOfWork();
                CurrentContext.Items[_requestUnitOfWork] = mgr;
            }

            return mgr;
        }


        /// <summary> Запуск приложения </summary>
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            IoC.BuildUp(SiteServicesConfiguration.Registries);

            ControllerBuilder.Current.SetControllerFactory(new GraphLabsControllerFactory(() => GetRequestUnitOfWork().Container));
        }


        /// <summary> Аутентификация </summary>
        protected void Application_AuthenticateRequest()
        {
            var container = GetRequestUnitOfWork().Container;

            var authSavingService = container.Resolve<IAuthenticationSavingService>();
            var membershipEngine = container.Resolve<IMembershipEngine>();

            var sessionInfo = authSavingService.GetSessionInfo();
            var success = membershipEngine.TryAuthenticate(sessionInfo.Email, sessionInfo.SessionGuid,
                Context.Request.GetClientIP());
            if (!success && !sessionInfo.IsEmpty())
            {
                authSavingService.SignOut();
            }
        }

        /// <summary> Начало запроса </summary>
        protected void Application_BeginRequest()
        {
            GetRequestUnitOfWork().OnRequestBeginning();
        }

        /// <summary> Запрос выполнен </summary>
        protected void Application_EndRequest()
        {
            GetRequestUnitOfWork().OnRequestSuccess();
        }

        /// <summary> Ошибка </summary>
        protected void Application_Error()
        {
            GetRequestUnitOfWork().OnRequestFailure();
        }
    }
}
