using System.Collections.Generic;
using GraphLabs.DomainModel;
using GraphLabs.Site.Core.Filters;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Groups
{
    public class GroupModel : IEntityBasedModel<Group>, IFilterableModel<Group>
    {
        public long Id { get; set; }

        [StringFilter("Номер группы")]
        public string Name { get; set; }

        public bool IsOpen { get; set; }

        public ICollection<Student> Students { get; set; }

        public int FirstYear { get; set; }

        public int Number { get; set; }
    }
}