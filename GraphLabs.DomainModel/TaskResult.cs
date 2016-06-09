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
    
    public partial class TaskResult : AbstractEntity
    {
        protected TaskResult()
        {
            this.StudentActions = new HashSet<StudentAction>();
        }
    
        public long Id { get; set; }
        public ExecutionStatus Status { get; set; }
        public int Score { get; set; }
    
        public virtual Result Result { get; set; }
        public virtual ICollection<StudentAction> StudentActions { get; set; }
        public virtual TaskVariant TaskVariant { get; set; }
    }
    
    #pragma warning restore 1591
    
}
