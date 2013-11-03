using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphLabs.Site.Models
{
    public class CreateLabModel
    {
        public string Name { get; set; }

        public DateTime AcquaintanceFrom { get; set; }

        public DateTime AcquaintanceTo { get; set; }

        public List<KeyValuePair<long, string>> Tasks { get; set; }
    }
}