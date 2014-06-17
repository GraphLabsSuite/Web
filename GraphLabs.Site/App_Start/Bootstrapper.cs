using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.DomainModel.Services;
using GraphLabs.Site.Logic;
using GraphLabs.Site.Logic.Security;
using GraphLabs.Site.Logic.Labs;
using GraphLabs.Site.Logic.Tasks;
using GraphLabs.Site.Models;
using GraphLabs.Tasks.Contract;
using Microsoft.Practices.Unity;
using Unity.Mvc4;

namespace GraphLabs.Site.App_Start
{
    /// <summary> Unity для Mvc4 </summary>
    public static class Bootstrapper
    {
        /// <summary> Инициализация. Дёргать на Application_Start в Global.asax </summary>
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            
            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();    
            RegisterTypes(container);

            return container;
        }

        /// <summary> Регистрируем компоненты </summary>
        private static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<ISystemDateService, SystemDateService>();
            
            container.RegisterType<IHashCalculator, BCryptCalculator>();
            
            container.RegisterType<IAuthenticationSavingService, FormsAuthenticationSavingService>();

            container.RegisterType<GraphLabsContext>(new PerRequestLifetimeManager());

            // ============================================================

            container.RegisterType<IDbContextManager, DbContextManager>(new PerRequestLifetimeManager());

            container.RegisterType<RepositoryFactory>(new PerRequestLifetimeManager());
            
            container.RegisterType<IGroupRepository>(new PerRequestLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetGroupRepository()));

            container.RegisterType<IUserRepository>(new PerRequestLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetUserRepository()));

            container.RegisterType<ISessionRepository>(new PerRequestLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetSessionRepository()));

            container.RegisterType<ILabRepository>(new PerRequestLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetLabRepository()));

            container.RegisterType<INewsRepository>(new PerRequestLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetNewsRepository()));
            
            container.RegisterType<ITaskRepository>(new PerRequestLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetTaskRepository()));

            container.RegisterType<IResultsRepository>(new PerRequestLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetResultsRepository()));

            // ============================================================

            container.RegisterType<IMembershipEngine, MembershipEngine>(new PerRequestLifetimeManager());

            container.RegisterType<ILabExecutionEngine, LabExecutionEngine>(new PerRequestLifetimeManager());
            
            container.RegisterType<INewsManager, NewsManager>(new PerRequestLifetimeManager());

            container.RegisterType<ITaskManager, TaskManager>(new PerRequestLifetimeManager());

            container.RegisterType<IResultsManager, ResultsManager>(new PerRequestLifetimeManager());

            // ============================================================

            container.RegisterType<IInitParamsProvider, InitParamsProvider>();
            container.RegisterType<ITaskExecutionModelFactory, TaskExecutionModelFactory>();
        }
    }
}