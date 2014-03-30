using System.Net;
using System.Web;

namespace GraphLabs.Site.Utils
{
    /// <summary> Ошибка 404 </summary>
    public class HttpNotFoundException : HttpException
    {
        /// <summary> Ошибка 404 </summary>
        public HttpNotFoundException()
            : base((int)HttpStatusCode.NotFound, "Страница не найдена.")
        {
        }
    }
}