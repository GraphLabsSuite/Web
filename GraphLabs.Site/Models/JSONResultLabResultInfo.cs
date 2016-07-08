using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GraphLabs.Dal.Ef;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Models
{
    public class TaskInfo
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Variant { get; set; }

        public int Result { get; set; }

        public TaskInfo(TaskResult[] tasks, int number)
        {
            Id = tasks[number].Id;
            Name = tasks[number].TaskVariant.Task.Name;
            Variant = "Вариант " + tasks[number].TaskVariant.Number;
            int? score = tasks[number].Score;
            if (score != null) Result = (int) score;
            else Result = -1;
        }
    }

    public class JSONResultLabResultInfo
    {
        public int Result { get; set; }

        public string LabName { get; set; }

        public TaskInfo[] Tasks { get; set; }

        public string[] Problems { get; set; }

        public long StudentsNumber { get; set; }

        public int Place { get; set; }
    }
}