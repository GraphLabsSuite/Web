using Microsoft.Practices.Unity;

namespace GraphLabs.Site.Models.Infrastructure
{
    /// <summary> Фабрика списочных моделей </summary>
    class ListModelLoader : IListModelLoader
    {
        private readonly IUnityContainer _container;

        /// <summary> Фабрика списочных моделей </summary>
        public ListModelLoader(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary> Создать модель списка </summary>
        public TListModel LoadListModel<TListModel, TModel>() where TListModel : IListModel<TModel>
        {
            return _container.Resolve<TListModel>();
        }
    }
}