//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NedbankRepository
{
    using System;
    using System.Collections.Generic;
    
    public partial class Widget
    {
        public long Id { get; set; }
        public string PageTypeId { get; set; }
        public string Description { get; set; }
        public string WidgetName { get; set; }
        public string DisplayName { get; set; }
        public bool IsConfigurable { get; set; }
        public string TenantCode { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public long UpdateBy { get; set; }
        public bool Instantiable { get; set; }
    }
}