using GraphLabs.Dal.Ef.Infrastructure;
using GraphLabs.Dal.Ef.Repositories;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.Site.Utils.IoC;
using Microsoft.Practices.Unity;

namespace GraphLabs.Dal.Ef.IoC
{
    /// <summary> Конфигурация Unity </summary>
    public class DomainModelEfConfiguration : IUnityRegistry
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
            container.RegisterType<IGraphLabsContext, GraphLabsContextWrapper>(new HierarchicalLifetimeManager());

            // устаревшие
            container.RegisterType<INewsContext, GraphLabsContextWrapper>(new HierarchicalLifetimeManager());
            container.RegisterType<IUsersContext, GraphLabsContextWrapper>(new HierarchicalLifetimeManager());
            container.RegisterType<ISessionsContext, GraphLabsContextWrapper>(new HierarchicalLifetimeManager());
            container.RegisterType<IReportsContext, GraphLabsContextWrapper>(new HierarchicalLifetimeManager());
            container.RegisterType<ITestsContext, GraphLabsContextWrapper>(new HierarchicalLifetimeManager());
            container.RegisterType<ILabWorksContext, GraphLabsContextWrapper>(new HierarchicalLifetimeManager());
            container.RegisterType<ITasksContext, GraphLabsContextWrapper>(new HierarchicalLifetimeManager());
            container.RegisterType<ISystemContext, GraphLabsContextWrapper>(new HierarchicalLifetimeManager());


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
            container.RegisterType<INewsRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetNewsRepository()));
            container.RegisterType<IResultsRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetResultsRepository()));
            container.RegisterType<ICategoryRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetCategoryRepository()));
            container.RegisterType<ISurveyRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetSurveyRepository()));
        }
    }
}
