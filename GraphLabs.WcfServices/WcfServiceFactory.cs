using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.DomainModel.Services;
using GraphLabs.Site.Logic;
using GraphLabs.Site.Logic.Tasks;
using GraphLabs.Site.Utils;
using GraphLabs.Site.Utils.XapProcessor;
using GraphLabs.WcfServices.Data;
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
            var mapper = CreateMapper();

            container.RegisterInstance(mapper);

            // register all your components with the container here
            container.RegisterType<ISystemDateService, SystemDateService>();

            container.RegisterType<GraphLabsContext>(new HierarchicalLifetimeManager());

            // ============================================================

            container.RegisterType<IChangesTracker, ChangesTracker>(new HierarchicalLifetimeManager());
            container.RegisterType<ITaskManager, TaskManager>(new HierarchicalLifetimeManager());
            container.RegisterType<IXapProcessor, XapProcessor>(new HierarchicalLifetimeManager());

            container.RegisterType<RepositoryFactory>(new HierarchicalLifetimeManager());

            container.RegisterType<IGroupRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetGroupRepository()));

            container.RegisterType<IUserRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetUserRepository()));

            container.RegisterType<ISessionRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetSessionRepository()));

            container.RegisterType<ILabRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetLabRepository()));

            container.RegisterType<ITaskRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetTaskRepository()));

            container.RegisterType<IResultsRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetResultsRepository()));

            // ============================================================

            container.RegisterType<IResultsManager, ResultsManager>(new HierarchicalLifetimeManager());

            // ============================================================

            container.RegisterType<ITasksDataService, TasksDataService>();
            container.RegisterType<ITaskDebugHelper, TaskDebugHelper>();
        }

        private Mapper CreateMapper()
        {
            var mapper = new Mapper();

            // Сюда можно положить специфичные для WCF маппинги
            mapper.CreateMap<TaskVariant, TaskVariantDto>();
            return mapper;
        }
    }    
}