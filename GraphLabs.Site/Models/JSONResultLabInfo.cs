using System.Collections.Generic;
using GraphLabs.Dal.Ef;
using System.Linq;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Models
{
    public class JSONResultLabInfo : JSONModel
    {
        public class ResultVariants
        {
            public long VarId { get; private set; }

            public string VarName { get; private set; }

            public List<KeyValuePair<long, string>> TasksVar { get; private set; }

            public bool IntrVar { get; set; }

            public ResultVariants(LabVariant variant)
            {
                VarId = variant.Id;
                VarName = variant.Number;
                TasksVar = MakeTasksVariantFromLabVariant(variant.TaskVariants);
                IntrVar = variant.IntroducingVariant;
            }

            private List<KeyValuePair<long, string>> MakeTasksVariantFromLabVariant(ICollection<TaskVariant> taskVariants)
            {
                var result = new List<KeyValuePair<long, string>>();
                foreach (var t in taskVariants)
                {
                    result.Add(new KeyValuePair<long, string>(t.Task.Id, t.Number));
                }
                return result;
            }
        }

        public long LabId { get; private set; }

        public string LabName { get; private set; }

        public List<KeyValuePair<long, string>> Tasks { get; private set; }

        public List<ResultVariants> Variants { get; private set; }

        public JSONResultLabInfo(LabWork lab)
            : base (0)
        {
            LabId = lab.Id;
            LabName = lab.Name;
            Tasks = MakeTasksFromLabEntry(lab.LabEntries);
            Variants = MakeVariantsFromLabVariants(lab.LabVariants);
        }

        private List<KeyValuePair<long, string>> MakeTasksFromLabEntry(ICollection<LabEntry> Entries)
        {
            var result = new List<KeyValuePair<long, string>>();
            foreach (var labEntry in Entries.OrderBy(e => e.Order))
            {
                result.Add(new KeyValuePair<long, string>(labEntry.Task.Id, labEntry.Task.Name));
            }
            return result;
        }

        private List<ResultVariants> MakeVariantsFromLabVariants(ICollection<LabVariant> variants)
        {
            var result = new List<ResultVariants>();
            foreach (var v in variants)
            {
                result.Add(new ResultVariants(v));
            }
            return result;
        }
    }
}