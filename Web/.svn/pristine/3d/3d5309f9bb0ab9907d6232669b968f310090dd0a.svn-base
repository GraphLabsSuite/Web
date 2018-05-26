using System.Collections.Generic;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.TestPoolEntry;

namespace GraphLabs.Site.Models.TestPool
{

    public class TestPoolModel : IEntityBasedModel<DomainModel.TestPool>
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public ICollection<TestPoolEntryModel> TestPoolEntries { get; set; }
    }
}
