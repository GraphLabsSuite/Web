using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphLabs.Site.Models
{
    public class TaskInfo
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Variant { get; set; }

        public int Result { get; set; }
    }

    public class JSONResultLabResultInfo
    {
        public int Result { get; set; }

        public string LabName { get; set; }

        public TaskInfo[] Tasks { get; set; }

        public string[] Problems { get; set; }

        public int StudentsNumber { get; set; }

        public int Place { get; set; }
    }
}