using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GraphLabs.DomainModel;
using GraphLabs.Dal.Ef;

namespace GraphLabs.Site.Models
{
    public class CreateLabModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public DateTime? AcquaintanceFrom { get; set; }

        public DateTime? AcquaintanceTo { get; set; }

        public List<KeyValuePair<long, string>> Tasks { get; set; }
		
        public CreateLabModel(long id, Task[] tasks)
        {
            Id = id;
            Tasks = MakeListFromTasks(tasks);
        }

        public CreateLabModel(LabWork lab)
        {
            Id = lab.Id;
            Name = lab.Name;
            AcquaintanceFrom = lab.AcquaintanceFrom;
            AcquaintanceTo = lab.AcquaintanceTill;
            Tasks = MakeListFromTasks( lab.LabEntries.Select(e => e.Task).ToArray() );
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