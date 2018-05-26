//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GraphLabs.DomainModel
{
    using System;
    using System.Collections.Generic;
    using GraphLabs.DomainModel.Infrastructure;
    
    
    #pragma warning disable 1591
    
    public partial class Task : AbstractEntity
    {
        protected Task()
        {
            this.TaskVariants = new HashSet<TaskVariant>();
        }
    
        public long Id { get; private set; }
        public string Name { get; set; }
        public byte[] VariantGenerator { get; set; }
        public string Sections { get; set; }
        public string Version { get; set; }
        public string Note { get; set; }
    
        public virtual ICollection<TaskVariant> TaskVariants { get; set; }
        public virtual TaskData TaskData { get; set; }
    }
    
    #pragma warning restore 1591
    
}
