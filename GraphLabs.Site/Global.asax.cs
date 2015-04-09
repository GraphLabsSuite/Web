using System;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GraphLabs.DomainModel;
using GraphLabs.Site.App_Start;
using GraphLabs.Site.Logic.Security;
using GraphLabs.Site.Models;
using GraphLabs.Site.Utils;
using Microsoft.Practices.Unity;

namespace GraphLabs.Site
{
    // Примечание: Инструкции по включению классического режима IIS6 или IIS7 
    // см. по ссылке http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public const string ContainerItemKey = "container";

        private IUnityContainer RequestContainer
        {
            get
            {
                if (Context == null)
                {
                    return null;
                }

                return Context.Items[ContainerItemKey] as IUnityContainer;
            }
            set
            {
                Context.Items[ContainerItemKey] = value;
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
            IoC.Initialise();

            ControllerBuilder.Current.SetControllerFactory(new GraphLabsControllerFactory(ContainerItemKey));
            SetupModelBinders();
        }

        private void SetupModelBinders()
        {
            var modelTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(GraphLabsModel).IsAssignableFrom(t) && !t.IsAbstract);
            foreach (var modelType in modelTypes)
            {
                ModelBinders.Binders.Add(modelType, new GraphLabsModelBinder(ContainerItemKey));
            }
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
            RequestContainer = IoC.GetChildContainer();
        }

        /// <summary> Запрос выполнен </summary>
        protected void Application_EndRequest()
        {
            DisposeContainer(true);
        }

        /// <summary> Ошибка </summary>
        protected void Application_Error()
        {
            DisposeContainer(false);
        }

        private void DisposeContainer(bool save)
        {
            var container = RequestContainer;
            if (container != null)
            {
                if (save)
                {
                    container.Resolve<IChangesTracker>().SaveChanges();
                }
                container.Dispose();
            }
        }
    }
}
