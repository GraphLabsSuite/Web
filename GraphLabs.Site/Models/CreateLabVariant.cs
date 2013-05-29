using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphLabs.Site.Models
{
    public class CreateLabVariant
    {
        public int Id { get; set; }

        public Dictionary<string, List<KeyValuePair<int, string>>> Variants { get; set; }
    }
}