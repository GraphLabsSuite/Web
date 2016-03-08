using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.EF;

namespace GraphLabs.Site.Models
{
    /// <summary> Данные о задании </summary>
    public class TaskVariantModel
    {
        /// <summary> Id </summary>
        public long Id { get; set; }

        [Display(Name = "Номер")]
        public string Number { get; set; }

        [Display(Name = "Версия варианта")]
        public long Version { get; set; }

        [Display(Name = "Версия генератора")]
        public string GeneratorVersion { get; set; }

        /// <summary> Ctor. </summary>
        public TaskVariantModel()
        {
        }

        /// <summary> Ctor. </summary>
        public TaskVariantModel(TaskVariant variant)
        {
            Contract.Requires(variant != null);

            Id = variant.Id;
            Number = variant.Number;
            Version = variant.Version;
            GeneratorVersion = variant.GeneratorVersion;
        }
    }
}