using System;

namespace GraphLabs.Site.Core.Filters
{
    [System.AttributeUsage(System.AttributeTargets.Property)]  
    public class DynamicBoundFilterAttribute  : Attribute, IFilterAttribute
    {
        private String _name;
        private Type _typeOfProvider;
        
        public DynamicBoundFilterAttribute(string name, Type typeOfProvider)
        {
            _name = name;
            _typeOfProvider = typeOfProvider;
        }
    }
}