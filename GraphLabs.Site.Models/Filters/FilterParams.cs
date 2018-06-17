﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Reflection;

namespace GraphLabs.Site.Core.Filters
{
    public class FilterParams<T>
    {
        private readonly NameValueCollection _filterParams;
        private Dictionary<string, object> cache = new Dictionary<string, object>();

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

        public DateTime? GetDateTimeParam(string name)
        {
            var val = GetParam(name);
            if (val == null) 
                return null;
            return DateTime.ParseExact(val, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
        }

        public object GetBoundedParam(string name)
        {
            var index = GetIntParam(name);
            if (index == null)
            {
                return null;
            }

            var propertyInfo = typeof(T).GetProperty(name);
            if (propertyInfo == null) return null;
            foreach (var customAttributeData in propertyInfo.CustomAttributes)
            {
                if (customAttributeData.AttributeType == typeof(BoundedFilterAttribute))
                {
                    return ((ReadOnlyCollection<CustomAttributeTypedArgument>) customAttributeData
                        .ConstructorArguments[1].Value)[index.Value].Value;
                } else if (customAttributeData.AttributeType == typeof(DynamicBoundFilterAttribute))
                {
                    if (cache.ContainsKey(name))
                    {
                        return cache[name];
                    }
                    var key = GetParam(name + "ver");
                    var values = (object[]) GraphLabsValuesHolder.getAndRemove(key);
                    var result = values[index.Value];
                    cache[name] = result;
                    return result;
                }
            }

            return null;
        }

        public int? GetIntParam(string name)
        {
            try
            {
                string s = GetParam(name);
                return s == null ? (int?) null : Int32.Parse(s);
            }
            catch (FormatException)
            {
                return null;
            }
        }


        public bool? GetBoolParam(string name)
        {
            string s = GetParam(name);
            return s == null ? (bool?) null : Boolean.Parse(s);
        }
    }
}