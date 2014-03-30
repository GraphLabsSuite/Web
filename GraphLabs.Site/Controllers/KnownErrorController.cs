using System.Net;
using System.Web.Mvc;

namespace GraphLabs.Site.Controllers
{
    [AllowAnonymous]
    public class KnownErrorController : GraphLabsController
    {
        //
        // GET: /KnownError/Error404
        public ActionResult Error404(string url)
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;

            var model = new Error404Model();
            
            model.RequestedUrl =
                Request.Url != null &&
                Request.Url.OriginalString.Contains(url) &&
                Request.Url.OriginalString != url
                    ? Request.Url.OriginalString
                    : url;

            model.ReferrerUrl =
                Request.UrlReferrer != null &&
                Request.UrlReferrer.OriginalString != model.RequestedUrl
                    ? Request.UrlReferrer.OriginalString
                    : null;

            return View(model);
        }

    }

    public class Error404Model
    {
        public string RequestedUrl { get; set; }
        public string ReferrerUrl { get; set; }
    }
}
