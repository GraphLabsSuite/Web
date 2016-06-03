using System;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Results
{
    public class ResultModel : IEntityBasedModel<Result>
    {
        public long Id { get; set; }
        public string Mode { get; set; }
        public DateTime StartDateTime { get; set; }
        public short? Score { get; set; }
        public string Status { get; set; }
        public string LabWorkName { get; set; }
        public string LabVariantNumber { get; set; }
    }
}
