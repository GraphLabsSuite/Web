using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphLabs.Site.Models
{
    public class CreateLabVariant
    {
        public int Id { get; set; }
        
        public string LabName { get; set; }

        public string VariantName { get; set; }

        public Dictionary<string, List<KeyValuePair<int, string>>> Variants { get; set; }
    }
}