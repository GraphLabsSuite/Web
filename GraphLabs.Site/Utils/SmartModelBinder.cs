using System;
using System.Web.Mvc;
using GraphLabs.Site.Models;
using GraphLabs.Site.Models.IoC;

namespace GraphLabs.Site.Utils
{
    public sealed class SmartModelBinder : DefaultModelBinder
    {
        public const string ModelTypeField = "ModelType";

        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var typeValue = bindingContext.ValueProvider.GetValue(ModelTypeField);
            var typeStr = (string)typeValue.ConvertTo(typeof(string));
            var type = typeof(ModelsConfiguration).Assembly.GetType(typeStr);
            var model = Activator.CreateInstance(type);
            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, type);
            return model;
        }
    }
}