using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GraphLabs.Site.App_Start;
using GraphLabs.Site.Logic.Security;
using GraphLabs.Site.ServicesConfig;
using GraphLabs.Site.Utils;
using GraphLabs.Site.Utils.Extensions;

namespace GraphLabs.Site
{
    /// <summary> Приложение GraphLabs - точка входа </summary>
    public class GraphLabsApplication : HttpApplication
    {
        private static readonly string _requestTransactionManagerKey = "transactionManager";

        private static HttpContext CurrentContext => HttpContext.Current;

        private static RequestTransactionManager GetRequestTransaction()
        {
            if (CurrentContext == null)
                return null;

            var mgr = (RequestTransactionManager)CurrentContext.Items[_requestTransactionManagerKey];
            if (mgr == null)
            {
                mgr = new RequestTransactionManager();
                CurrentContext.Items[_requestTransactionManagerKey] = mgr;
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
            IoC.BuildUp();

            ControllerBuilder.Current.SetControllerFactory(new GraphLabsControllerFactory(() => GetRequestTransaction().Container));
        }


        /// <summary> Аутентификация </summary>
        protected void Application_AuthenticateRequest()
        {
            using (var container = IoC.GetChildContainer())
            {
                var authSavingService = container.Resolve<IAuthenticationSavingService>();
                var membershipEngine = container.Resolve<IMembershipEngine>();
                
                var sessionInfo = authSavingService.GetSessionInfo();
                var success = membershipEngine.TryAuthenticate(sessionInfo.Email, sessionInfo.SessionGuid, Context.Request.GetClientIP());
                if (!success && !sessionInfo.IsEmpty())
                {
                    authSavingService.SignOut();
                }
            }
        }

        /// <summary> Начало запроса </summary>
        protected void Application_BeginRequest()
        {
            GetRequestTransaction().OnRequestBeginning();
        }

        /// <summary> Запрос выполнен </summary>
        protected void Application_EndRequest()
        {
            GetRequestTransaction().OnRequestSuccess();
        }

        /// <summary> Ошибка </summary>
        protected void Application_Error()
        {
            GetRequestTransaction().OnRequestFailure();
        }
    }
}
