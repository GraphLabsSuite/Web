using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphLabs.Site.Models
{
    public class StudentsResultModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public string Variant { get; set; }

        public int Result { get; set; }
    }
}