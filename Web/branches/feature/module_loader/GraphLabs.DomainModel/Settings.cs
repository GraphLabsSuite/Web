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
    
    public partial class Settings : AbstractEntity
    {
        protected Settings()
        {
        }
    
        public long Id { get; private set; }
        public System.DateTime SystemDate { get; set; }
        public byte[] DefaultVariantGenerator { get; set; }
    }
    
    #pragma warning restore 1591
    
}
