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
    
    public partial class LabWork : AbstractEntity
    {
        protected LabWork()
        {
            this.LabVariants = new HashSet<LabVariant>();
            this.LabEntries = new HashSet<LabEntry>();
            this.Groups = new HashSet<Group>();
        }
    
        public long Id { get; private set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> AcquaintanceFrom { get; set; }
        public Nullable<System.DateTime> AcquaintanceTill { get; set; }
    
        public virtual ICollection<LabVariant> LabVariants { get; set; }
        public virtual ICollection<LabEntry> LabEntries { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
    
    #pragma warning restore 1591
    
}
