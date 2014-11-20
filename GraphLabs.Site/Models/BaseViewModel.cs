using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GraphLabs.Site.Models
{
    public class BaseViewModel
    {
        /// <summary> IOC-контейнер </summary>
        protected IDependencyResolver DependencyResolver
        {
            get { return System.Web.Mvc.DependencyResolver.Current; }
        }
    }
}