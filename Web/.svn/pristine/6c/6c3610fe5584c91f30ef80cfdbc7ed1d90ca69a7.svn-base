using System;
using System.Collections.Generic;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.Lab;

namespace GraphLabs.Site.Models.CreateLab
{
    public class CreateLabModel : LabModel, IEntityBasedModel<LabWork>
    {
        public List<KeyValuePair<long, string>> Tasks { get; set; }

        public CreateLabModel() { }

        public CreateLabModel(long id, Task[] tasks)
        {
            Id = id;
            Tasks = MakeListFromTasks(tasks);
        }

        private List<KeyValuePair<long, string>> MakeListFromTasks(Task[] tasks)
        {
            var result = new List<KeyValuePair<long, string>>();
            foreach (var t in tasks)
            {
                result.Add(new KeyValuePair<long, string>(t.Id, t.Name));
            }
            return result;
        }
    }
}