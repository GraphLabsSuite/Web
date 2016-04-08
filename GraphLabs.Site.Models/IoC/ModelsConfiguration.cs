using System;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.DemoLab;
using GraphLabs.Site.Models.Groups;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.News;
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
            container.RegisterType<IEntityBasedModelLoader<DemoLabModel, DomainModel.LabWork>, DemoLabModelLoader>(new PerResolveLifetimeManager());
        }
    }
}
