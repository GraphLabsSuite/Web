using System.Collections.Generic;
using GraphLabs.DomainModel;
using Newtonsoft.Json;

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

        public long LabId { get; set; }

        public TaskExecutionModel[] Tasks { get; set; }

        public int currentTask { get; set; }

        public LabWorkExecutionModel(string labName, long labId, TaskVariant[] variants)
        {
            LabName = labName;
            LabId = labId;
            Tasks = new TaskExecutionModel[variants.Length];
            for (int i = 0; i < variants.Length; ++i)
            {
                Tasks[i] = new TaskExecutionModel(variants[i].Task, variants[i].Id);
            }            
        }

        public void SetCurrent(int num)
        {
            currentTask = num;
        }
    }
}