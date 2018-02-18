using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using GraphLabs.Site.Core;
using GraphLabs.Site.Core.Filters;
using Microsoft.Practices.Unity;

namespace ASP.Helpers
{
    public class GraphLabsWebGrid : WebGrid
    {
        private IEnumerable<dynamic> _source;

        public GraphLabsWebGrid(IEnumerable<dynamic> source)
            : base(source, canPage: true, canSort: true, rowsPerPage: 10)
        {
            _source = source;
        }

        public IHtmlString GetHtml(string tableStyle = "webGrid", string headerStyle = "webgrid-header",
            string footerStyle = "webgrid-footer",
            string rowStyle = "webgrid-row", string alternatingRowStyle = "webgrid-altrow",
            string selectedRowStyle = "webgrid-selected-row",
            string caption = null, bool displayHeader = true, bool fillEmptyRows = false,
            string emptyRowCellValue = null, IEnumerable<WebGridColumn> columns = null,
            IEnumerable<string> exclusions = null,
            WebGridPagerModes mode = WebGridPagerModes.Numeric | WebGridPagerModes.NextPrevious,
            string firstText = null, string previousText = null, string nextText = null, string lastText = null,
            int numericLinksCount = 5, object htmlAttributes = null)
        {
            var buildHtml = new StringBuilder();
            addHtmlFilters(buildHtml);
            buildHtml.AppendLine(base.GetHtml(tableStyle, headerStyle, footerStyle, rowStyle, alternatingRowStyle,
                selectedRowStyle, caption, displayHeader, fillEmptyRows, emptyRowCellValue, columns, exclusions, mode,
                firstText, previousText, nextText, lastText, numericLinksCount, htmlAttributes
            ).ToString());
            
            return new HtmlString(buildHtml.ToString());
        }

        private void addHtmlFilters(StringBuilder stringBuilder)
        {
            var types = new FiltersInfo().GetFilters();
            var mainDiv = new HtmlGenericControl("div");
            var form = GraphLabsUIFactory.createHtmlForm();
            mainDiv.Controls.Add(form);
            
            foreach (var type in types)
            {
                if (isA(_source, type))
                {
                    MethodInfo textMethod = null;
                    foreach (var method in type.GetMethods())
                    {
                        if (method.Name.EndsWith("Text"))
                        {
                            textMethod = method;
                        }
                    }
                    var m = _source.GetType().GetMethod(textMethod.Name);
                    var s = m.Invoke(_source, null);
                    form.Controls.Add(GraphLabsUIFactory.createInputField(type.Name.Split(new[]{"By", "`"}, StringSplitOptions.None)[1], (string)s));
                }
            }

            if (form.Controls.Count == 0)
            {
                return;
            }

            form.Controls.Add(GraphLabsUIFactory.createHtmlResetButton());
            form.Controls.Add(GraphLabsUIFactory.createHtmlSubmitButton());
            mainDiv.RenderControl(new HtmlTextWriter(new StringWriter(stringBuilder)));
        }

        private static bool isA(object o, Type t)
        {
            return (o.GetType().GetInterfaces().Any(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == t));
        }
    }
}