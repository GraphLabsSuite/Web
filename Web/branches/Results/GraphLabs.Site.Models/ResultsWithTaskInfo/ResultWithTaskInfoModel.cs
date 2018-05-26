using GraphLabs.Site.Models.Results;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.StudentActions;
using GraphLabs.Site.Models.TaskResultsWithActions;
using GraphLabs.Site.Models.TaskResults;
using System.Collections.Generic;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.Groups;


namespace GraphLabs.Site.Models.ResultsWithTaskInfo
{
    public class ResultWithTaskInfoModel : ResultModel
    {
        public GroupModel[] Group { get; set; }
        public TaskResultModel[] TaskResults { get; set; }
        public StudentActionModel[] StudentActions { get; set; }
        public TaskResultWithActionsModel[] TaskResultWithActions { get; set; }
        public Result Result { get; set; }
        public long StudentId { get; set; }
        public string TaskName { get; set; }
        public string StudentName { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
