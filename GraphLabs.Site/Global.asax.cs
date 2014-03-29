using System;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GraphLabs.DomainModel;
using GraphLabs.Site.App_Start;
using GraphLabs.Site.Logic.Security;
using GraphLabs.Site.Utils;

namespace GraphLabs.Site
{
    // Примечание: Инструкции по включению классического режима IIS6 или IIS7 
    // см. по ссылке http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static TransactionManager TransactionManager
        {
            get
            {
                return DependencyResolver.Current.GetService<TransactionManager>();
            }
        }

        private static IAuthenticationSavingService AuthSavingService
        {
            get
            {
                return DependencyResolver.Current.GetService<IAuthenticationSavingService>();
            }
        }

        private static IMembershipEngine MembershipEngine
        {
            get
            {
                return DependencyResolver.Current.GetService<IMembershipEngine>();
            }
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
            Bootstrapper.Initialise();
        }

        /// <summary> Аутентификация </summary>
        protected void Application_AuthenticateRequest()
        {
            var sessionInfo = AuthSavingService.GetSessionInfo();
            var success = MembershipEngine.TryAuthenticate(sessionInfo.Email, sessionInfo.SessionGuid, Context.Request.GetClientIP());
            if (!success && !sessionInfo.IsEmpty())
            {
                AuthSavingService.SignOut();
            }
        }

        /// <summary> Начало запроса </summary>
        protected void Application_BeginRequest()
        {
            TransactionManager.BeginTransaction();
        }

        /// <summary> Запрос выполнен </summary>
        protected void Application_EndRequest()
        {
            if (TransactionManager.HasActiveTransaction)
            {
                TransactionManager.Commit();
            }
            DisposeContextItems();
        }

        /// <summary> Ошибка </summary>
        protected void Application_Error()
        {
            TransactionManager.Rollback();

            DisposeContextItems();
        }

        private void DisposeContextItems()
        {
            if (Context == null)
            {
                return;
            }

            var disposableItems = Context.Items.Values
                .OfType<IDisposable>();
            foreach (var item in disposableItems)
            {
                item.Dispose();
            }
        }
    }
}
