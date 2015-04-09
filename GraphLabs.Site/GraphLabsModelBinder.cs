using System;
using System.Web;
using System.Web.Mvc;
using GraphLabs.Site.Models;
using Microsoft.Practices.Unity;

namespace GraphLabs.Site
{
    /// <summary> Биндер моделей </summary>
    class GraphLabsModelBinder : DefaultModelBinder
    {
        private readonly string _containerKey;

        public GraphLabsModelBinder(string containerKey)
        {
            _containerKey = containerKey;
        }

        private IUnityContainer RequestContainer
        {
            get { return HttpContext.Current.Items[_containerKey] as IUnityContainer; }
        }

        /// <summary> Создает заданный тип модели, используя указанные контекст контроллера и контекст привязки. </summary>
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            return typeof(GraphLabsModel).IsAssignableFrom(modelType) 
                ? RequestContainer.Resolve(modelType) 
                : base.CreateModel(controllerContext, bindingContext, modelType);
        }
    }
}