using System;
using System.Collections.Generic;

namespace GraphLabs.Site.Core.Filters
{
    [AttributeUsage(AttributeTargets.Property)]  
    public class BoundedFilterAttribute : Attribute, IFilterAttribute
    {
        private string _name;
        private object[] _limiters;
      
        public BoundedFilterAttribute(string name, object[] limiters)
        {
            _name = name;
            _limiters = limiters; 
        }
    }
}