using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.TestPoolEntry
{
    public class SaveTestPoolEntryModel : IEntityBasedModel<DomainModel.TestPoolEntry>
    {
        public long Id { get; set; }

        public int Score { get; set; }

        public ScoringStrategy ScoringStrategy { get; set; }

        public long TestQuestion { get; set; }

        public long TestPool { get; set; }
    }
}