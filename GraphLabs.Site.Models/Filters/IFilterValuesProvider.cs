using System;

namespace GraphLabs.Site.Core.Filters
{
    public interface IFilterValuesProvider
    {
        Object[] getValues();
    }
}