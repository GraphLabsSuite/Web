using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Models
{
    public class DemoLabModel
    {
        public long LabWorkId { get; set; }

        public string LabWorkName { get; set; }

        public List<KeyValuePair<long, string>> LabVariants { get; set; }

        public DateTime AcquaintanceTill { get; set; }

        /// <summary> Конструктор </summary>
        public DemoLabModel(LabWork lab, LabVariant[] variants)
        {
            LabWorkId = lab.Id;
            LabWorkName = lab.Name;
            LabVariants = new List<KeyValuePair<long, string>>();
            foreach (var lv in variants)
            {
                LabVariants.Add(new KeyValuePair<long, string>(lv.Id, lv.Number));
            }
            AcquaintanceTill = (DateTime)lab.AcquaintanceTill;
        }
    }
}