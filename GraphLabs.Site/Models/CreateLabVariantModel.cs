using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Models
{
    public class CreateLabVariantModel
    {
        [Key]
        public long id { get; set; }

        public string Name { get; set; }

        public Dictionary<string, List<KeyValuePair<long, string>>> Variant { get; set; }
    }
}