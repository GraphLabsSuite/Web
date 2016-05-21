using System;
using System.Collections.Generic;
using GraphLabs.Site.Models.Lab;

namespace GraphLabs.Site.Models.DemoLab
{
    public class DemoLabModel : LabModel
    {
        public ICollection<KeyValuePair<long, string>> Variants { get; set; }

        public DateTime AcquaintanceTill { get; set; }
    }
}