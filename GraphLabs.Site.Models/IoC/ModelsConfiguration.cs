using GraphLabs.DomainModel;
using GraphLabs.Site.Models.DemoLabs;
using GraphLabs.Site.Models.Groups;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.News;
using GraphLabs.Site.Models.Results;
using GraphLabs.Site.Models.ResultsWithTaskInfo;
using GraphLabs.Site.Models.StudentActions;
using GraphLabs.Site.Models.TaskResults;
using GraphLabs.Site.Models.TaskResultsWithActions;
using GraphLabs.Site.Utils.IoC;
using Microsoft.Practices.Unity;

namespace GraphLabs.Site.Models.IoC
{
    /// <summary> Конфигуратор моделей </summary>
    public class ModelsConfiguration : IUnityRegistry
    {
        public void ConfigureContainer(IUnityContainer container)
        {
            container.RegisterType<ITaskExecutionModelFactory, TaskExecutionModelFactory>(new PerResolveLifetimeManager());

            container.RegisterType<IListModelLoader, ListModelLoader>(new HierarchicalLifetimeManager());

            // группы
            container.RegisterType<IEntityBasedModelSaver<GroupModel, Group>, GroupModelSaver>(new PerResolveLifetimeManager());
            container.RegisterType<IEntityBasedModelLoader<GroupModel, Group>, GroupModelLoader>(new PerResolveLifetimeManager());

            // новости
            container.RegisterType<IEntityBasedModelSaver<NewsModel, DomainModel.News>, NewsModelSaver>(new PerResolveLifetimeManager());
            container.RegisterType<IEntityBasedModelLoader<NewsModel, DomainModel.News>, NewsModelLoader>(new PerResolveLifetimeManager());

            // демолабы
            container.RegisterType<IEntityBasedModelLoader<DemoLabModel, LabWork>, DemoLabModelLoader>(new PerResolveLifetimeManager());

            // результаты лаб
            container.RegisterType<IEntityBasedModelLoader<ResultModel, Result>, ResultModelLoader>(new PerResolveLifetimeManager());

            // результаты лаб с инфой о заданиях
            container.RegisterType<IEntityBasedModelLoader<ResultWithTaskInfoModel, Result>, ResultWithTaskInfoModelLoader>(new PerResolveLifetimeManager());

            // результаты заданий
            container.RegisterType<IEntityBasedModelLoader<TaskResultModel, TaskResult>, TaskResultModelLoader>(new PerResolveLifetimeManager());

            // результаты заданий с инфой о действиях
            container.RegisterType<IEntityBasedModelLoader<TaskResultWithActionsModel, TaskResult>, TaskResultWithActionsModelLoader>(new PerResolveLifetimeManager());

            // действия студентов
            container.RegisterType<IEntityBasedModelLoader<StudentActionModel, StudentAction>, StudentActionModelLoader>(new PerResolveLifetimeManager());
        }
    }
}
