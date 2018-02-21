using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using GraphLabs.Site.Core.Filters;

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
            AddHtmlFilters(buildHtml);
            buildHtml.AppendLine(base.GetHtml(tableStyle, headerStyle, footerStyle, rowStyle, alternatingRowStyle,
                selectedRowStyle, caption, displayHeader, fillEmptyRows, emptyRowCellValue, columns, exclusions, mode,
                firstText, previousText, nextText, lastText, numericLinksCount, htmlAttributes
            ).ToString());
            
            return new HtmlString(buildHtml.ToString());
        }

        private void AddHtmlFilters(StringBuilder stringBuilder)
        {
            var mainDiv = new HtmlGenericControl("div");
            var form = GraphLabsUIFactory.CreateHtmlForm();
            mainDiv.Controls.Add(form);
            
            if (IsA(_source, typeof(IFilterable<,>)))
            {
                var e = GetType(_source, typeof(IFilterable<,>));
               
                var genericArgument = e.GetGenericArguments()[1];
                foreach (var propertyInfo in genericArgument.GetProperties())
                {
                    foreach (var customAttributeData in propertyInfo.CustomAttributes)
                    {
                        if (customAttributeData.AttributeType.GetInterfaces().Contains(typeof(IFilterAttribute)))
                        {
                            if (propertyInfo.PropertyType == typeof(bool))
                            {
                                form.Controls.Add(GraphLabsUIFactory.CreateInputCheckBox(propertyInfo.Name,
                                    (string) customAttributeData.ConstructorArguments[0].Value));
                            } else {
                                form.Controls.Add(GraphLabsUIFactory.CreateInputField(propertyInfo.Name, 
                                    (string) customAttributeData.ConstructorArguments[0].Value));
                            }
                        }
                    }
                }
            }

            if (form.Controls.Count == 0)
            {
                return;
            }

            form.Controls.Add(GraphLabsUIFactory.CreateHtmlResetButton());
            form.Controls.Add(GraphLabsUIFactory.CreateHtmlSubmitButton());
            mainDiv.RenderControl(new HtmlTextWriter(new StringWriter(stringBuilder)));
        }

        private static Type GetType(object o, Type t)
        {
            return (o.GetType().GetInterfaces().FirstOrDefault(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == t));
        }

        private static bool IsA(object o, Type t)
        {
            return GetType(o, t) != null;
        }
    }
}