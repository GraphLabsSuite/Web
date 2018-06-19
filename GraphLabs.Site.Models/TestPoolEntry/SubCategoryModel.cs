using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.TestPool
{
    public class SubCategoryModel : IEntityBasedModel<SubCategory>
    {
        public long Id { get; set; }
        public long CategoryId { get; set; }

        public string CategoryName { get; set; }
        public string Name { get; set; }
    }
}