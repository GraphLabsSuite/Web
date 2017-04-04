using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.TestPoolEntry
{
    public class AuxTestPoolEntryModel
    {
        public long Id { get; set; }

        public string Type { get; set; }

        public int Value { get; set; }
    }
}