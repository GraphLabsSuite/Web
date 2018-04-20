using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace GraphLabs.Site.Core.Filters
{
    [System.AttributeUsage(System.AttributeTargets.Property)]  
    public class BoundedFilterAttribute : Attribute, IFilterAttribute
    {
        private String _name;
        private Object[] _limiters;
      
        public BoundedFilterAttribute(string name, Object[] limiters)
        {
            _name = name;
            _limiters = limiters; 
        }
    }
}