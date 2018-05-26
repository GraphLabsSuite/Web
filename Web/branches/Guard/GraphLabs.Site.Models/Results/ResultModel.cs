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
        public int? Score { get; set; }
        public string Status { get; set; }
        public string LabWorkName { get; set; }
        public string LabVariantNumber { get; set; }

        private DateTime? _dateFrom;
        private DateTime? _dateTill;

        public ResultModel FilterByDate(DateTime? from, DateTime? till)
        {
            if (from.HasValue && till.HasValue && till < from)
                throw new ArgumentOutOfRangeException();

            _dateFrom = from?.Date;
            _dateTill = till?.Date.AddDays(1).AddMilliseconds(-1);

            return this;
        }
    }
}
