using System;
using System.Collections.Generic;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Lab
{
    public class LabModel : IEntityBasedModel<LabWork>
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}