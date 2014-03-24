using System.Collections.Generic;
using GraphLabs.DomainModel;
using System.Linq;

namespace GraphLabs.Site.Models
{
    public class JSONResultLabInfo
    {
        public class ResultVariants
        {
            public long VarId { get; private set; }

            public string VarName { get; private set; }

            public List<KeyValuePair<long, string>> TasksVar { get; private set; }

            public ResultVariants(LabVariant variant)
            {
                VarId = variant.Id;
                VarName = variant.Number;
                TasksVar = MakeTasksVariantFromLabVariant(variant.TaskVariants);
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

        public int Result { get; private set; }

        public long LabId { get; private set; }

        public string LabName { get; private set; }

        public List<KeyValuePair<long, string>> Tasks { get; private set; }

        public List<ResultVariants> Variants { get; private set; }

        /// <summary> Конструктор для создания объекта, свидетельствующего об ошибке </summary>
        public JSONResultLabInfo(int error)
        {
            Result = error;
        }

        public JSONResultLabInfo(LabWork lab)
        {
            Result = 0;
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