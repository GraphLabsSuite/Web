using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphLabs.Site.Models
{
    public class JSONResultCreateLab
    {
        public int Result { get; set; }

        public long LabId { get; set; }

        public string LabName { get; set; }

        public JSONResultCreateLab(int result, string labName, long labId = 0)
        {
            Result = result;
            LabId = labId;
            LabName = labName;
        }
    }
}