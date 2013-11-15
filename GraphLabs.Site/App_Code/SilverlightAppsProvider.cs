using System.IO;
using System.Linq;
using System.Web;
using GraphLabs.DomainModel;

namespace GraphLabs.Site
{
    public class SilverlightAppsProvider : IHttpHandler
    {
        #region Реализация IHttpHandler

        private readonly GraphLabsContext _ctx = new GraphLabsContext();

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            var appKind = request.QueryString["kind"];
            long taskId;
            if (!long.TryParse(request.QueryString["taskId"], out taskId))
                return;

            if (appKind == "generator")
            {
                ProvideGenerator(taskId, context.Response);
            }
        }

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.
        /// </returns>
        public bool IsReusable
        {
            get { return true; }
        }

        #endregion // Реализация IHttpHandler


        #region Вспомагательные функции

        private void ProvideGenerator(long taskId, HttpResponse response)
        {
            var task = _ctx.Tasks.Find(taskId);
            if (task == null)
                return;

            response.ContentType = "application/x-silverlight-app";
            var generator = task.VariantGenerator;
            if (generator == null)
            {
                generator = _ctx.Settings.Single().DefaultVariantGenerator;
                if (generator == null)
                {
                    return;
                }
            }

            using (var writer = new BinaryWriter(response.OutputStream))
            {
                writer.Write(generator);
            }
        }

        ///// <summary> Записывает текст svg в SvgText для дальнейшего отображения на странице. </summary>
        //private void OutputSvg(XElement root, HttpResponse response)
        //{
        //    ChecksHelper.CheckArgumentNotNull(root, "root");

        //    root.SetAttributeValue("width", ConfigurationManager.AppSettings["MapWidth"]);
        //    root.SetAttributeValue("height", ConfigurationManager.AppSettings["MapHeight"]);

        //    var layersNamespace = root.GetNamespaceOfPrefix("v");
        //    var properties = root.Descendants(layersNamespace + "documentProperties").FirstOrDefault();
        //    if (properties != null)
        //    {
        //        properties.Remove();
        //    }
        //    response.ContentType = "image/svg+xml";
        //    var xws = new XmlWriterSettings
        //    {
        //        CheckCharacters = true,
        //        CloseOutput = true,
        //        ConformanceLevel = ConformanceLevel.Document,
        //        Encoding = Encoding.UTF8,
        //        Indent = true,
        //        IndentChars = "    ",
        //        OmitXmlDeclaration = true,
        //        NamespaceHandling = NamespaceHandling.OmitDuplicates,
        //        NewLineChars = "\r\n",
        //        NewLineHandling = NewLineHandling.Replace,
        //        NewLineOnAttributes = false,
        //    };
        //    using (var xw = XmlWriter.Create(response.Output, xws))
        //    {
        //        root.Save(xw);
        //    }
        //}

        #endregion // Вспомагательные функции
    }
}