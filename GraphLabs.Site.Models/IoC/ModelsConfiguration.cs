using GraphLabs.DomainModel;
using GraphLabs.Site.Models.CreateLab;
using GraphLabs.Site.Models.DemoLab;
using GraphLabs.Site.Models.Groups;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.Lab;
using GraphLabs.Site.Models.LabExecution;
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
            container.RegisterType<IListModelLoader, ListModelLoader>(new HierarchicalLifetimeManager());

            // группы
            container.RegisterType<IEntityBasedModelSaver<GroupModel, Group>, GroupModelSaver>(new PerResolveLifetimeManager());
            container.RegisterType<IEntityBasedModelLoader<GroupModel, Group>, GroupModelLoader>(new PerResolveLifetimeManager());

            // новости
            container.RegisterType<IEntityBasedModelSaver<NewsModel, DomainModel.News>, NewsModelSaver>(new PerResolveLifetimeManager());
            container.RegisterType<IEntityBasedModelLoader<NewsModel, DomainModel.News>, NewsModelLoader>(new PerResolveLifetimeManager());

            // лабы
            container.RegisterType<IEntityBasedModelLoader<LabModel, LabWork>, LabModelLoader>(new PerResolveLifetimeManager());

            // демолабы
            container.RegisterType<IEntityBasedModelLoader<DemoLabModel, LabWork>, DemoLabModelLoader>(new PerResolveLifetimeManager());

            // создание лабы
            container.RegisterType<IEntityBasedModelLoader<CreateLabModel, LabWork>, CreateLabModelLoader>(new PerResolveLifetimeManager());

            // выполнение лабы
            container.RegisterType<IDemoVariantModelLoader, DemoVariantModelLoader>(new HierarchicalLifetimeManager());
        }
    }
}
