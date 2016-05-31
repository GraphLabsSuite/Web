using System.Collections.Generic;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.StudentActions;

namespace GraphLabs.Site.Models.TaskResults
{
    class TaskResultModel : IEntityBasedModel<TaskResult>
    {
        public long Id { get; set; }
        public IEnumerable<StudentActionModel> StudentActions { get; set; }
        public string TaskVariantNumber { get; set; }
        public ExecutionStatus Status { get; set; }
    }
}
