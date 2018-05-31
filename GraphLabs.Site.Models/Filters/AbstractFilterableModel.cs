using System;
using System.Linq.Expressions;

namespace GraphLabs.Site.Core.Filters
{
    public abstract class AbstractFilterableModel<T>
    {
        public static Expression<Func<T, bool>> CreateFilter(FilterParams<T> filterParams)
        {
            return t => true;
        }
    }
}