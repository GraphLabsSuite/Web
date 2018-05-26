using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.Site.Models.Infrastructure;
using System.Web.Script.Serialization;

namespace GraphLabs.Site.Models.StudentAnswer
{
    public class StudentAnswersModel
    {

        public long LabVarId { get; set; }

        public long TestPoolEntryId { get; set; }

        public List<long> ChosenAnswerIds { get; set; }

        public long TestResultId { get; set; }
    }
}
