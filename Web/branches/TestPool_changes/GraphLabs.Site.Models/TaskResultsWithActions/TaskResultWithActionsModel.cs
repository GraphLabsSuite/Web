using GraphLabs.DomainModel;
using GraphLabs.Site.Models.StudentActions;
using GraphLabs.Site.Models.TaskResults;

namespace GraphLabs.Site.Models.TaskResultsWithActions
{
    public class TaskResultWithActionsModel : TaskResultModel
    {
        public StudentActionModel[] StudentActions { get; set; }
        public Result Result { get; set; }
    }
}