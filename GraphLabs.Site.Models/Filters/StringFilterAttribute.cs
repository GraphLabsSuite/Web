using System;

namespace GraphLabs.Site.Core.Filters
{
    [System.AttributeUsage(System.AttributeTargets.Property)]  
    public class StringFilterAttribute : Attribute, IFilterAttribute
    {
        private string _name;

        public StringFilterAttribute(string name)
        {
            _name = name;
        }
    }
}