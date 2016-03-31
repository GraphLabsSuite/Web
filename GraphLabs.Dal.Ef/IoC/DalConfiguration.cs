using GraphLabs.Dal.Ef.Infrastructure;
using GraphLabs.Dal.Ef.OperationContext;
using GraphLabs.Dal.Ef.Repositories;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Site.Utils.IoC;
using Microsoft.Practices.Unity;

namespace GraphLabs.Dal.Ef.IoC
{
    /// <summary> Конфигурация Unity </summary>
    public sealed class DalConfiguration : IUnityRegistry
    {
        /// <summary> Сконфигурировать </summary>
        public void ConfigureContainer(IUnityContainer container)
        {
            // Сам EF-контекст
            container.RegisterType<GraphLabsContext>(new HierarchicalLifetimeManager());


            // Инфраструктура
            container.RegisterType<IChangesTracker, ChangesTracker>(new HierarchicalLifetimeManager());
            container.RegisterType<ISystemDateService, SystemDateService>(new PerResolveLifetimeManager());

            // Контексты предметной области
            container.RegisterInstance<IOperationContextFactory<IGraphLabsContext>>(new OperationContextFactory());
            container.RegisterType<IEntityQuery, GraphLabsContextImpl>();

            // устаревшие
            container.RegisterType<INewsContext, GraphLabsContextsAggregator>(new HierarchicalLifetimeManager());
            container.RegisterType<IUsersContext, GraphLabsContextsAggregator>(new HierarchicalLifetimeManager());
            container.RegisterType<ISessionsContext, GraphLabsContextsAggregator>(new HierarchicalLifetimeManager());
            container.RegisterType<IReportsContext, GraphLabsContextsAggregator>(new HierarchicalLifetimeManager());
            container.RegisterType<ITestsContext, GraphLabsContextsAggregator>(new HierarchicalLifetimeManager());
            container.RegisterType<ILabWorksContext, GraphLabsContextsAggregator>(new HierarchicalLifetimeManager());
            container.RegisterType<ITasksContext, GraphLabsContextsAggregator>(new HierarchicalLifetimeManager());
            container.RegisterType<ISystemContext, GraphLabsContextsAggregator>(new HierarchicalLifetimeManager());


            // Старые репозитории
            container.RegisterType<RepositoryFactory>(new HierarchicalLifetimeManager());

            container.RegisterType<IGroupRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetGroupRepository()));
            container.RegisterType<IUserRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetUserRepository()));
            container.RegisterType<ISessionRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetSessionRepository()));
            container.RegisterType<ILabRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetLabRepository()));
            container.RegisterType<IResultsRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetResultsRepository()));
            container.RegisterType<ICategoryRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetCategoryRepository()));
            container.RegisterType<ISurveyRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetSurveyRepository()));
        }
    }
}
