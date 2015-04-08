using System.Data.Entity;

namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> Лабораторные </summary>
    public interface ILabWorksContext
    {
        /// <summary> Лабораторные работы </summary>
        DbSet<LabWork> LabWorks { get; set; }
        
        /// <summary> Варианты лабораторных </summary>
        DbSet<LabVariant> LabVariants { get; set; }

        /// <summary> Задание, входящее в состав работы </summary>
        DbSet<LabEntry> LabEntries { get; set; }
    }
}