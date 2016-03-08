using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.EF;

namespace GraphLabs.Site.Models
{
    public class CreateLabVariantModel
    {
        [Key]
        public long id { get; set; }

        public long varId { get; set; }

        public string Name { get; set; }

        public Dictionary<KeyValuePair<long, string>, List<KeyValuePair<long, string>>> Variant { get; set; }

        public CreateLabVariantModel(LabWork lab, long variantId = 0)
        {
            id = lab.Id;
            varId = variantId;
            Name = lab.Name;
            Variant = MakeLabVariantModel(lab.LabEntries.Select(e => e.Task));
        }

        private Dictionary<KeyValuePair<long, string>, List<KeyValuePair<long, string>>> MakeLabVariantModel(IEnumerable<Task> tasks)
        {
            var result = new Dictionary<KeyValuePair<long, string>, List<KeyValuePair<long, string>>>();
            foreach (var t in tasks)
            {
                result.Add(new KeyValuePair<long, string>(t.Id, t.Name), MakeTaskVariantsList(t.TaskVariants));
            }
            return result;
        }

        private List<KeyValuePair<long, string>> MakeTaskVariantsList(ICollection<TaskVariant> taskVariants)
        {
            var result = new List<KeyValuePair<long, string>>();
            foreach (var tv in taskVariants)
            {
                result.Add(new KeyValuePair<long, string>(tv.Id, tv.Number));
            }
            return result;
        }
    }
}