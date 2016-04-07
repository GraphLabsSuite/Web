using System;
using System.Collections.Generic;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.LabWorks
{
    public class LabWorkModel : IEntityBasedModel<LabWork>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? AcquaintanceFrom { get; set; }
        public DateTime? AcquaintanceTill { get; set; }

        public virtual ICollection<LabVariant> LabVariants { get; set; }
    }
}
