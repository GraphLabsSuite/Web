using System.Collections.Generic;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.LabVariants
{
    public class LabVariantModel : IEntityBasedModel<LabVariant>
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public long Version { get; set; }
        public bool IntroducingVariant { get; set; }

        public ICollection<TestQuestion> TestQuestions { get; set; }
        public ICollection<TaskVariant> TaskVariants { get; set; }
        public long LabWorkId { get; set; }
        public ICollection<Result> Results { get; set; }
    }
}
