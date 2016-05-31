using System;
using System.Collections.Generic;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.LabWorks
{
    public class LabWorkModel : IEntityBasedModel<DomainModel.LabWork>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? AcquaintanceFrom { get; set; }
        public DateTime? AcquaintanceTill { get; set; }

        public ICollection<long> LabVariantIds { get; set; }
        public ICollection<LabEntry> LabEntries { get; set; }
        public ICollection<Group> Groups { get; set; }
    }
}
