using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Models
{
    public class JSONResultEditVariant
    {
        public long Result { get; set; }

        public string Name { get; set; }

        public List<KeyValuePair<long, long>> Variant { get; set; }

        /// <summary> Конструктор для создания объекта, свидетельствующего об ошибке </summary>
        public JSONResultEditVariant(long error)
        {
            Result = error;
        }

        public JSONResultEditVariant(long result, LabVariant variant)
        {
            Result = result;
            Name = variant.Number;
            Variant = MakeVariantFromTasksVariant(variant.TaskVariants);
        }

        private List<KeyValuePair<long, long>> MakeVariantFromTasksVariant(ICollection<TaskVariant> taskVariants)
        {
            var result = new List<KeyValuePair<long, long>>();
            foreach (var t in taskVariants)
            {
                result.Add(new KeyValuePair<long, long>(t.Task.Id, t.Id));
            }
            return result;
        }
    }
}