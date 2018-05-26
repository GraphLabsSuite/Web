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
    
    public partial class Result : AbstractEntity
    {
        protected Result()
        {
            this.AbstractResultEntries = new HashSet<AbstractResultEntry>();
        }
    
        public long Id { get; private set; }
        public LabExecutionMode Mode { get; set; }
        public System.DateTime StartDateTime { get; private set; }
        public Nullable<int> Score { get; set; }
        public ExecutionStatus Status { get; set; }
    
        public virtual Student Student { get; set; }
        public virtual LabVariant LabVariant { get; set; }
        public virtual ICollection<AbstractResultEntry> AbstractResultEntries { get; set; }
    }
    
    #pragma warning restore 1591
    
}
