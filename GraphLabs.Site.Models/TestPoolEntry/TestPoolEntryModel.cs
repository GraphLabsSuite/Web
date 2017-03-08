using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.TestPool;

namespace GraphLabs.Site.Models.TestPoolEntry
{
    public class TestPoolEntryModel : IEntityBasedModel<DomainModel.TestPoolEntry>
    {
        public long Id { get; set; }

        public int Score { get; set; }

        public ScoringStrategy ScoringStrategy { get; set; }

        public TestQuestion TestQuestion { get; set; }

        public ICollection<TestResult> TestResults { get; set; }
    }
}
