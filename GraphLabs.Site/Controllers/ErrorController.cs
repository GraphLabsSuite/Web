using System;
using System.Net;
using System.Web.Mvc;
using GraphLabs.Site.Models.Error;

namespace GraphLabs.Site.Controllers
{
    /// <summary> Ошибка </summary>
    [AllowAnonymous]
    public class ErrorController : GraphLabsController
    {
        //
        // GET: /Error/Error404
        public ActionResult Error404(string url)
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;

            var model = new Error404Model();

            model.RequestedUrl =
                Request.Url != null &&
                (url == null || Request.Url.OriginalString.Contains(url) && Request.Url.OriginalString != url)
                    ? Request.Url.OriginalString
                    : url ?? string.Empty;

            model.ReferrerUrl =
                Request.UrlReferrer != null &&
                Request.UrlReferrer.OriginalString != model.RequestedUrl
                    ? Request.UrlReferrer.OriginalString
                    : null;

            return View(model);
        }

        //
        // GET: /Error/Oops
        public ActionResult Oops(Exception exception)
        {
            var model = exception != null
                ? new OopsModel { Exception = exception.ToString(), ShortDescription = exception.Message }
                : new OopsModel
                {
                    Exception = "Подробная информация отсутствует.",
                    ShortDescription = "Неизвестная ошибка."
                };

            return View(model);
        }

    }
}
