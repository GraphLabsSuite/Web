using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphLabs.Site.Models
{
    public class JSONResultEditVariant
    {
        public long Result { get; set; }

        public string Name { get; set; }

        public List<KeyValuePair<long, long>> Variant { get; set; }
    }
}