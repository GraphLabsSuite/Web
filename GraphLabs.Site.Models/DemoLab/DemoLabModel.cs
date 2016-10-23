using System;
using System.Collections.Generic;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.Lab;

namespace GraphLabs.Site.Models.DemoLab
{
    public class DemoLabModel : LabModel, IEntityBasedModel<AbstractLabSchedule>
    {
        public ICollection<KeyValuePair<long, string>> Variants { get; set; }

        public DateTime AcquaintanceTill { get; set; }
    }
}