using System;
using System.Collections.Generic;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.DemoLabs
{
    public class DemoLabModel : IEntityBasedModel<DomainModel.LabWork>
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public ICollection<KeyValuePair<long, string>> Variants { get; set; }

        public DateTime AcquaintanceTill { get; set; }
    }
}