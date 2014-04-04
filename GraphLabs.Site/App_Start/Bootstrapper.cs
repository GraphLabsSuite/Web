using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.DomainModel.Services;
using GraphLabs.Site.Logic.Security;
using GraphLabs.Site.Logic.Labs;
using Microsoft.Practices.Unity;
using Unity.Mvc4;

namespace GraphLabs.Site.App_Start
{
    /// <summary> Unity ��� Mvc4 </summary>
    public static class Bootstrapper
    {
        /// <summary> �������������. ĸ����� �� Application_Start � Global.asax </summary>
        /// <returns></returns>
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

        /// <summary> ������������ ���������� </summary>
        private static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<ISystemDateService, SystemDateService>();
            
            container.RegisterType<IHashCalculator, BCryptCalculator>();
            
            container.RegisterType<IAuthenticationSavingService, FormsAuthenticationSavingService>(
                new InjectionConstructor(typeof(ISystemDateService)));

            container.RegisterType<GraphLabsContext>(new PerRequestLifetimeManager(),
                new InjectionFactory(c => new GraphLabsContext()));

            // ============================================================

            container.RegisterType<DbContextManager>(new PerRequestLifetimeManager(),
                new InjectionFactory(c => new DbContextManager(c.Resolve<GraphLabsContext>())));

            container.RegisterType<RepositoryFactory>(new PerRequestLifetimeManager(),
                new InjectionFactory(c => new RepositoryFactory(c.Resolve<GraphLabsContext>(), c.Resolve<ISystemDateService>())));
            
            container.RegisterType<IGroupRepository>(new PerRequestLifetimeManager(), 
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetGroupRepository()));

            container.RegisterType<IUserRepository>(new PerRequestLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetUserRepository()));

            container.RegisterType<ISessionRepository>(new PerRequestLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetSessionRepository()));

            container.RegisterType<ILabRepository>(new PerRequestLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetLabRepository()));

            // ============================================================

            container.RegisterType<IMembershipEngine, MembershipEngine>(new PerRequestLifetimeManager(),
                new InjectionConstructor(
                    typeof(DbContextManager),
                    typeof(IHashCalculator), 
                    typeof(ISystemDateService), 
                    typeof(IUserRepository), 
                    typeof(IGroupRepository),
                    typeof(ISessionRepository)));

            container.RegisterType<IDemoLabEngine, DemoLabEngine>(new PerRequestLifetimeManager(),
                new InjectionConstructor(
                    typeof(ISystemDateService),
                    typeof(ILabRepository)));

            container.RegisterType<ILabExecutionEngine, LabExecutionEngine>(new PerRequestLifetimeManager(),
                new InjectionConstructor(
                    typeof(ILabRepository)));

        }
    }
}