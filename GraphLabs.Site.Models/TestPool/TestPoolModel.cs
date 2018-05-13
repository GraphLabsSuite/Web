using GraphLabs.DomainModel.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.TestPoolEntry;

namespace GraphLabs.Site.Models.TestPool
{

    public class TestPoolModel : IEntityBasedModel<DomainModel.TestPool>
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public ICollection<TestPoolEntryModel> TestPoolEntries { get; set; }

        public string Category { get; set; }
        public string SubCategory { get; set; }

    }
}
