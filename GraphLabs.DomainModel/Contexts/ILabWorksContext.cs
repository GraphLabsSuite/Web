namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> Лабораторные </summary>
    public interface ILabWorksContext
    {
        /// <summary> Лабораторные работы </summary>
        IEntitySet<LabWork> LabWorks { get; set; }
        
        /// <summary> Варианты лабораторных </summary>
        IEntitySet<LabVariant> LabVariants { get; set; }

        /// <summary> Задание, входящее в состав работы </summary>
        IEntitySet<LabEntry> LabEntries { get; set; }
    }
}