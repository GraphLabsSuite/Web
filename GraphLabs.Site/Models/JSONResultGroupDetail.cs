using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphLabs.Site.Models
{
    public class StudentResult
    {
        public string Name { get; set; }

        public int? Mark { get; set; }

        public int? Result { get; set; }

        public KeyValuePair<long, int>[] Tasks { get; set; }
    }

    public class JSONResultGroupDetail
    {
        public int Result { get; set; }

        public string Name { get; set; }

        public StudentResult[] Students { get; set; }

        public KeyValuePair<long, string>[] LabEntry { get; set; }
    }
}