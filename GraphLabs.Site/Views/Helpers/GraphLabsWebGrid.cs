﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using GraphLabs.Site;
using GraphLabs.Site.Core.Filters;
using GraphLabs.Site.ServicesConfig;
using Microsoft.Practices.Unity;

namespace ASP.Helpers
{
    public class GraphLabsWebGrid : WebGrid
    {
        private IEnumerable<dynamic> _source;
        private readonly string[] _filtersToDiplay;

        public GraphLabsWebGrid(IEnumerable<dynamic> source, params string[] filtersToDiplay)
            : base(source, canPage: true, canSort: true, rowsPerPage: 10)
        {
            _source = source;
            _filtersToDiplay = filtersToDiplay;
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

        //by default display all filters
        private bool mustDisplayFilterFor(string fieldName)
        {
            if (_filtersToDiplay.Length == 0)
            {
                return true;
            }
            return _filtersToDiplay.Contains(fieldName);
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
                    if (mustDisplayFilterFor(propertyInfo.Name))
                    {
                        foreach (var customAttributeData in propertyInfo.CustomAttributes)
                        {
                            if (customAttributeData.AttributeType.GetInterfaces().Contains(typeof(IFilterAttribute)))
                            {
                                if (customAttributeData.AttributeType == typeof(StringFilterAttribute))
                                {
                                    //фильтрация по одному полю с подписью
                                    if (propertyInfo.PropertyType == typeof(bool))
                                    {
                                        form.Controls.Add(GraphLabsUIFactory.CreateInputCheckBox(propertyInfo.Name,
                                            (string) customAttributeData.ConstructorArguments[0].Value));
                                    }
                                    else if (propertyInfo.PropertyType == typeof(DateTime))
                                    {
                                        form.Controls.Add(GraphLabsUIFactory.CreateInputDateTimeField(propertyInfo.Name,
                                            (string) customAttributeData.ConstructorArguments[0].Value));
                                    }
                                    else
                                    {
                                        form.Controls.Add(GraphLabsUIFactory.CreateInputField(propertyInfo.Name,
                                            (string) customAttributeData.ConstructorArguments[0].Value));
                                    }
                                }
                                else if (customAttributeData.AttributeType == typeof(BoundedFilterAttribute))
                                {
                                    //фильтрация с подписанным фильтруемым полем с ограниченным набором значений
                                    var options =
                                        ((ReadOnlyCollection<CustomAttributeTypedArgument>) customAttributeData
                                            .ConstructorArguments[1].Value)
                                        .Select(a => a.Value)
                                        .Select((v, i) => new {Key = i, Value = v})
                                        .ToDictionary(o => o.Key.ToString(), o => o.Value.ToString());
                                    form.Controls.Add(GraphLabsUIFactory.CreateSelectField(propertyInfo.Name,
                                        (string) customAttributeData.ConstructorArguments[0].Value, options));
                                }
                                else if (customAttributeData.AttributeType == typeof(DynamicBoundFilterAttribute))
                                {
                                    //фильтрация динамически по генерации опций в run-time
                                    var type = (Type) customAttributeData.ConstructorArguments[1].Value;
                                    var ioc = GraphLabsApplication.GetRequestUnitOfWork().Container;
                                    var filters = ((IFilterValuesProvider) ioc.Resolve(type))
                                        .getValues();
                                    var key = GraphLabsValuesHolder.registerValue(filters);
                                    var options = filters
                                        .Select((v, i) => new {Key = i, Value = v})
                                        .ToDictionary(o => o.Key.ToString(), o => o.Value.ToString());
                                    form.Controls.Add(
                                        GraphLabsUIFactory.CreateHiddenField(propertyInfo.Name + "ver", key));
                                    form.Controls.Add(GraphLabsUIFactory.CreateSelectField(propertyInfo.Name,
                                        (string) customAttributeData.ConstructorArguments[0].Value, options));
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