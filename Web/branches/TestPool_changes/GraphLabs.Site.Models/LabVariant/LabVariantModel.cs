using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.Lab;

namespace GraphLabs.Site.Models.LabVariant
{
    public class LabVariantModel : IEntityBasedModel<DomainModel.LabVariant>
    {
        public long Id { get; set; }

        public long Number { get; set; }

        public long Version { get; set; }

        public bool IntroducingVariant { get; set; }

        public LabWork LabWork { get; set; }

        public DomainModel.TestPool TestPool { get; set; }
    }
}
