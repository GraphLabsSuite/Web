using System;
using System.Linq;
using System.Reflection;

namespace GraphLabs.Site.Core
{
    public class FiltersInfo
    {
        public Type[] GetFilters()
        {
            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                where t.IsInterface && t.Namespace == "GraphLabs.Site.Core.Filters"
                select t;
            return q.ToArray();
        }
    }
}