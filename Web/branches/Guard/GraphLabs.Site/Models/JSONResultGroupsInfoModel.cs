using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphLabs.Site.Models
{
    public class GroupResult
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int StudentsCount { get; set; }

        public int Count5 { get; set; }

        public int Count4 { get; set; }

        public int Count3 { get; set; }

        public int Count2 { get; set; }

        public int Count0 { get; set; }
    }

    public class JSONResultGroupsInfoModel
    {
        public int Result { get; set; }

        public string LabName { get; set; }

        public GroupResult[] Marks { get; set; }
    }
}