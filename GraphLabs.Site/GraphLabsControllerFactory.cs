using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GraphLabs.Site.Controllers;
using Microsoft.Practices.Unity;

namespace GraphLabs.Site
{
    /// <summary> Фабрика контроллеров </summary>
    public class GraphLabsControllerFactory : DefaultControllerFactory
    {
        private readonly string _containerKey;

        public GraphLabsControllerFactory(string containerKey)
        {
            _containerKey = containerKey;
        }

        private IUnityContainer RequestContainer
        {
            get { return HttpContext.Current.Items[_containerKey] as IUnityContainer; }
        }

        /// <summary> Извлекает экземпляр контроллера для заданного контекста запроса и типа контроллера. </summary>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            try
            {
                if (controllerType != null)
                {
                    return RequestContainer.Resolve(controllerType) as IController;
                }

                return base.GetControllerInstance(requestContext, null);
            }
            catch (HttpException ex)
            {
                switch (ex.GetHttpCode())
                {
                    case (int)HttpStatusCode.NotFound:
                        var errorController = new ErrorController();
                        errorController.InvokeHttp404(requestContext.HttpContext);
                        return errorController;
                    default:
                        throw;
                }
            }
        }
    }
}