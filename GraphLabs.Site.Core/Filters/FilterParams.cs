using System;
using System.Collections.Specialized;

namespace GraphLabs.Site.Core.Filters
{
    public class FilterParams
    {
        private readonly NameValueCollection _filterParams;

        public FilterParams(NameValueCollection filterParams)
        {
            _filterParams = filterParams;
        }

        private string GetParam(string name)
        {
            string value = _filterParams.Get(name);
            if (value == "") value = null;
            return value;
        }

        public string GetStringParam(string name)
        {
            return GetParam(name);
        }
        
        public bool? GetBoolParam(string name)
        {
            string s = GetParam(name);
            return s == null ? (bool?) null : Boolean.Parse(s);
        }
    }
}