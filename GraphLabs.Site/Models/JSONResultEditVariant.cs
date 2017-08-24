using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GraphLabs.DomainModel;
using GraphLabs.Dal.Ef;

namespace GraphLabs.Site.Models
{
    public class JSONResultEditVariant
    {
        public string Name { get; set; }

        public List<KeyValuePair<long, long>> Variant { get; set; }

        public bool IntroducingVariant { get; set; }

        public long TestPoolId { get; set; }

        public JSONResultEditVariant(DomainModel.LabVariant variant)
        {
            Name = variant.Number;
            Variant = MakeVariantFromTasksVariant(variant.TaskVariants);
            IntroducingVariant = variant.IntroducingVariant;
            TestPoolId = (variant.TestPool != null) ? variant.TestPool.Id : -1;
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