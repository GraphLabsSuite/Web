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
    
    public partial class TaskVariant : AbstractEntity
    {
        protected TaskVariant()
        {
        }
    
        public long Id { get; set; }
        public string Number { get; set; }
        public string GeneratorVersion { get; set; }
        public byte[] Data { get; set; }
        public long Version { get; private set; }
    
        public virtual Task Task { get; set; }
    }
    
    #pragma warning restore 1591
    
}
