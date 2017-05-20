using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.StudentAnswer
{
    public class StudentAnswerModel : IEntityBasedModel<DomainModel.StudentAnswer>
    {
        public long ChosenAnswerId { get; set; }

        public long TestResultId { get; set; }
    }
}
