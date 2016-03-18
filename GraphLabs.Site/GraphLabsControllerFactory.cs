using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GraphLabs.Site.Controllers;
using GraphLabs.Site.Utils.Extensions;
using IDependencyResolver = GraphLabs.Site.Utils.IoC.IDependencyResolver;

namespace GraphLabs.Site
{
    /// <summary> Фабрика контроллеров </summary>
    public class GraphLabsControllerFactory : DefaultControllerFactory
    {
        private readonly Func<IDependencyResolver> _getContainer;

        public GraphLabsControllerFactory(Func<IDependencyResolver> containerFactory)
        {
            _getContainer = containerFactory;
        }

        /// <summary> Извлекает экземпляр контроллера для заданного контекста запроса и типа контроллера. </summary>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            try
            {
                if (controllerType != null)
                {
                    return _getContainer().Resolve(controllerType) as IController;
                }

                return base.GetControllerInstance(requestContext, null);
            }
            catch (HttpException ex)
            {
                switch (ex.GetHttpCode())
                {
                    case (int)HttpStatusCode.NotFound:
                        var errorController = _getContainer().Resolve<ErrorController>();
                        errorController.InvokeHttp404(requestContext.HttpContext);
                        return errorController;
                    default:
                        throw;
                }
            }
        }
    }
}