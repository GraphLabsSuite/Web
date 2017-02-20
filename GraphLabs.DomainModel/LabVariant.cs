//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GraphLabs.DomainModel
{
    using System;
    using System.Collections.Generic;
    using GraphLabs.DomainModel.Infrastructure;
    
    
    #pragma warning disable 1591
    
    public partial class LabVariant : AbstractEntity
    {
        protected LabVariant()
        {
            this.IntroducingVariant = false;
            this.TaskVariants = new HashSet<TaskVariant>();
            this.TestPools = new HashSet<TestPool>();
        }
    
        public long Id { get; private set; }
        public string Number { get; set; }
        public long Version { get; set; }
        public bool IntroducingVariant { get; set; }
    
        public virtual ICollection<TaskVariant> TaskVariants { get; set; }
        public virtual LabWork LabWork { get; set; }
        public virtual ICollection<TestPool> TestPools { get; set; }
    }
    
    #pragma warning restore 1591
    
}
