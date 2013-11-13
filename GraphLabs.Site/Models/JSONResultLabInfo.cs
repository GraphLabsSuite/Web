using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphLabs.Site.Models
{
    public class JSONResultVariants
    {
        public long VarId { get; set; }

        public string VarName { get; set; }

        public List<KeyValuePair<int, string>> TasksVar { get; set; }
    }

    public class JSONResultLabInfo
    {
        public int Result { get; set; }

        public long LabId { get; set; }

        public string LabName { get; set; }

        public List<KeyValuePair<int, string>> Tasks { get; set; }

        public JSONResultVariants[] Variants { get; set; }
    }
}