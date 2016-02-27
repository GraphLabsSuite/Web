using GraphLabs.DomainModel.EF;
using GraphLabs.DomainModel.EF.Contexts;
using GraphLabs.DomainModel.EF.Repositories;
using GraphLabs.DomainModel.EF.Services;
using GraphLabs.Site.Logic;
using GraphLabs.Site.Logic.Security;
using GraphLabs.Site.Logic.Tasks;
using GraphLabs.Site.Models;
using GraphLabs.Site.Utils.XapProcessor;
using GraphLabs.Tasks.Contract;
using Microsoft.Practices.Unity;

namespace GraphLabs.Site.App_Start
{
    /// <summary> Unity </summary>
    public static class IoC
    {
        private static IUnityContainer _container;

        /// <summary> Инициализация. Дёргать на Application_Start в Global.asax </summary>
        public static void Initialise()
        {
            if (_container == null)
            {
                _container = BuildUnityContainer();
            }
        }

        public static IUnityContainer GetChildContainer()
        {
            return _container.CreateChildContainer();
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            RegisterTypes(container);
            return container;
        }

        /// <summary> Регистрируем компоненты </summary>
        private static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<ISystemDateService, SystemDateService>(new PerResolveLifetimeManager());
            
            container.RegisterType<IHashCalculator, BCryptCalculator>(new PerResolveLifetimeManager());
            
            container.RegisterType<IAuthenticationSavingService, FormsAuthenticationSavingService>(new PerResolveLifetimeManager());

            container.RegisterType<IXapProcessor, XapProcessor>(new PerResolveLifetimeManager());

            container.RegisterType<IInitParamsProvider, InitParamsProvider>(new PerResolveLifetimeManager());

            container.RegisterType<ITaskExecutionModelFactory, TaskExecutionModelFactory>(new PerResolveLifetimeManager());

            // ============================================================

            container.RegisterType<GraphLabsContext>(new HierarchicalLifetimeManager());

            container.RegisterType<INewsContext>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<GraphLabsContext>()));
            container.RegisterType<IUsersContext>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<GraphLabsContext>()));
            container.RegisterType<ISessionsContext>(new HierarchicalLifetimeManager(), 
                new InjectionFactory(c => c.Resolve<GraphLabsContext>()));
            container.RegisterType<IReportsContext>(new HierarchicalLifetimeManager(), 
                new InjectionFactory(c => c.Resolve<GraphLabsContext>()));
            container.RegisterType<ITestsContext>(new HierarchicalLifetimeManager(), 
                new InjectionFactory(c => c.Resolve<GraphLabsContext>()));
            container.RegisterType<ILabWorksContext>(new HierarchicalLifetimeManager(), 
                new InjectionFactory(c => c.Resolve<GraphLabsContext>()));
            container.RegisterType<ITasksContext>(new HierarchicalLifetimeManager(), 
                new InjectionFactory(c => c.Resolve<GraphLabsContext>()));
            container.RegisterType<ISystemContext>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<GraphLabsContext>()));

            // ============================================================

            container.RegisterType<IChangesTracker, ChangesTracker>(new HierarchicalLifetimeManager());

            container.RegisterType<RepositoryFactory>(new HierarchicalLifetimeManager());

            container.RegisterType<IGroupRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetGroupRepository()));

            container.RegisterType<IUserRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetUserRepository()));

            container.RegisterType<ISessionRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetSessionRepository()));

            container.RegisterType<ILabRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetLabRepository()));

            container.RegisterType<INewsRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetNewsRepository()));

            container.RegisterType<IResultsRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetResultsRepository()));

            container.RegisterType<ICategoryRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetCategoryRepository()));

            container.RegisterType<ISurveyRepository>(new HierarchicalLifetimeManager(),
                            new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetSurveyRepository()));

            // ============================================================

            container.RegisterType<IMembershipEngine, MembershipEngine>(new HierarchicalLifetimeManager());

            container.RegisterType<INewsManager, NewsManager>(new HierarchicalLifetimeManager());

            container.RegisterType<ITaskManager, TaskManager>(new HierarchicalLifetimeManager());

            container.RegisterType<IResultsManager, ResultsManager>(new HierarchicalLifetimeManager());

            // ============================================================
        }
    }
}