using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GraphLabs.Site.Controllers;

namespace GraphLabs.Site
{
    /// <summary> Фабрика контроллеров </summary>
    public class GraphLabsControllerFactory : DefaultControllerFactory
    {
        /// <summary> Извлекает экземпляр контроллера для заданного контекста запроса и типа контроллера. </summary>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            try
            {
                return base.GetControllerInstance(requestContext, controllerType);
            }
            catch (HttpException ex)
            {
                switch (ex.GetHttpCode())
                {
                    case (int)HttpStatusCode.NotFound:
                        var errorController = new KnownErrorController();
                        errorController.InvokeHttp404(requestContext.HttpContext);
                        return errorController;
                    default:
                        throw;
                }
            }
        }
    }
}