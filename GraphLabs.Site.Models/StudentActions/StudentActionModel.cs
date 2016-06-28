using System;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.StudentActions
{
    public class StudentActionModel : IEntityBasedModel<StudentAction>
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public int Penalty { get; set; }
        public DateTime Time { get; set; }
    }
}
