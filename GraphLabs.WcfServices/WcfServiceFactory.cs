using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.DomainModel.Services;
using GraphLabs.Site.Logic;
using GraphLabs.Tasks.Contract;
using Microsoft.Practices.Unity;
using Unity.Wcf;

namespace GraphLabs.WcfServices
{
    /// <summary> Фабрика сервисов с поддержкой Unity </summary>
	public class WcfServiceFactory : UnityServiceHostFactory
    {
        /// <summary> Сконфигурировать зависимости </summary>
        protected override void ConfigureContainer(IUnityContainer container)
        {
			// register all your components with the container here
            container.RegisterType<ISystemDateService, SystemDateService>();

            container.RegisterType<GraphLabsContext>(new HierarchicalLifetimeManager());

            // ============================================================

            container.RegisterType<IDbContextManager, DbContextManager>(new HierarchicalLifetimeManager());

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

            container.RegisterType<ITaskRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetTaskRepository()));

            container.RegisterType<IResultsRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetResultsRepository()));

            // ============================================================

            container.RegisterType<IResultsManager, ResultsManager>(new HierarchicalLifetimeManager());

            // ============================================================

            container.RegisterType<IInitParamsProvider, InitParamsProvider>();

            // ============================================================

            container.RegisterType<ITasksDataService, TasksDataService>();
        }
    }    
}