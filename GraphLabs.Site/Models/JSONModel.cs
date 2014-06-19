using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphLabs.Site.Models
{
    public class JSONModel
    {
        public int Result { get; set; }

        /// <summary> Конструктор для создания объекта </summary>
        public JSONModel(int result)
        {
            Result = result;
        }
    }
}