using System.Collections.Generic;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.StudentActions;

namespace GraphLabs.Site.Models.TaskResults
{
    public class TaskResultModel : IEntityBasedModel<TaskResult>
    {
        public long Id { get; set; }
        public string TaskName { get; set; }
        public string TaskVariantNumber { get; set; }
        public string Status { get; set; }
    }
}
