using System;
using GraphLabs.Dal.Ef.Infrastructure;
using GraphLabs.Dal.Ef.Repositories;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.Dal.Ef;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.Site.Logic;
using GraphLabs.Site.Logic.Tasks;
using GraphLabs.Site.Logic.XapParsing;
using GraphLabs.Site.Utils;
using GraphLabs.WcfServices.Data;
using GraphLabs.WcfServices.DebugTaskUploader;
using Microsoft.Practices.Unity;

namespace GraphLabs.WcfServices.Infrastructure
{
    static class IoC
    {
        /// <summary> Сконфигурировать зависимости </summary>
        public static void Configure(IUnityContainer container)
        {
            var mapper = CreateMapper();

            container.RegisterInstance(mapper);

            // register all your components with the container here
            container.RegisterType<ISystemDateService, SystemDateService>();

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

            container.RegisterType<INewsRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetNewsRepository()));

            container.RegisterType<IResultsRepository>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => c.Resolve<RepositoryFactory>().GetResultsRepository()));

            // ============================================================

            container.RegisterType<IResultsManager, ResultsManager>(new HierarchicalLifetimeManager());

            // ============================================================

            container.RegisterType<ITasksDataService, TasksDataService>();
            container.RegisterType<IDebugTaskUploader, DebugTaskUploader.DebugTaskUploader>();
        }

        private static Mapper CreateMapper()
        {
            var mapper = new Mapper();

            // Сюда можно положить специфичные для WCF маппинги
            mapper.CreateMap<TaskVariant, TaskVariantDto>();
            return mapper;
        }
    }
}
