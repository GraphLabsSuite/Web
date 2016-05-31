using System;
using System.Collections.Generic;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.TaskResults;

namespace GraphLabs.Site.Models.Results
{
    class ResultModel : IEntityBasedModel<Result>
    {
        public long Id { get; set; }
        public LabExecutionMode Mode { get; set; }
        public DateTime StartDateTime { get; set; }
        public short? Score { get; set; }

        public Student Student { get; set; }
        public string LabVariantNumber { get; set; }
        public IEnumerable<TaskResultModel> TaskResults { get; set; }
    }
}
