using GraphLabs.Site.Models.StudentActions;
using GraphLabs.Site.Models.TaskResults;

namespace GraphLabs.Site.Models.LabExecution
{
    public class TaskResultWithActionsModel : TaskResultModel
    {
        public StudentActionModel[] StudentActions { get; set; }
        public long ResultId { get; set; }
    }
}
