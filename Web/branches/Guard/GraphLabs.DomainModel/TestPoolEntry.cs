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
    
    public partial class TestPoolEntry : AbstractEntity
    {
        protected TestPoolEntry()
        {
        }
    
        public long Id { get; set; }
        public int Score { get; set; }
        public ScoringStrategy ScoringStrategy { get; set; }
    
        public virtual TestQuestion TestQuestion { get; set; }
        public virtual TestPool TestPool { get; set; }
    }
    
    #pragma warning restore 1591
    
}
