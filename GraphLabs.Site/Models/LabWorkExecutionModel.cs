using System.Collections.Generic;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Models
{
    public class LabWorkExecutionModel
    {
        public class TaskExecutionModel
        {
            public string TaskName { get; set; }

            public long TaskId { get; set; }

            public long TaskVarId { get; set; }

            public bool IsSolved { get; set; }

            public TaskExecutionModel(Task task, long taskVarId)
            {
                TaskName = task.Name;
                TaskId = task.Id;
                TaskVarId = taskVarId;
                IsSolved = false;
            }
        }

        public string LabName { get; set; }

        public List<TaskExecutionModel> Tasks { get; set; }

        public LabWorkExecutionModel(string labName)
        {
            LabName = labName;
            Tasks = new List<TaskExecutionModel>();
        }

        public void AddTask(Task task, long taskVarId)
        {
            Tasks.Add(new TaskExecutionModel(task, taskVarId));
        }
    }
}