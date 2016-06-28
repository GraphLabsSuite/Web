using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.Site.Models.StudentActions;
using GraphLabs.Site.Models.TaskResults;

namespace GraphLabs.Site.Models.TaskResultsWithActions
{
    public class TaskResultWithActionsModel : TaskResultModel
    {
        public StudentActionModel[] StudentActions { get; set; }
        public long ResultId { get; set; }
    }
}
