using System;
using System.Data.SqlClient;
using GraphLabs.Site.Core.Filters;

namespace GraphLabs.Site.Models.Groups
{
    public class NameFilterProvider : IFilterValuesProvider
    {   
        public object[] getValues()
        {
            //open connection to db
            //do another logic and finally
            //but dinamycally
            Random r = new Random();
            return new object[]{"hello" + r.NextDouble().ToString() , "world" + r.NextDouble().ToString()};
        }
    }
}