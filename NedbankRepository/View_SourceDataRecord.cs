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
    
    public partial class View_SourceDataRecord
    {
        public long Id { get; set; }
        public long StatementId { get; set; }
        public long CustomerId { get; set; }
        public string AccountId { get; set; }
        public Nullable<long> PageWidgetId { get; set; }
        public Nullable<long> PageId { get; set; }
        public Nullable<long> WidgetId { get; set; }
        public System.DateTime EventDate { get; set; }
        public string EventType { get; set; }
        public string TenantCode { get; set; }
        public string PageName { get; set; }
        public string WidgetName { get; set; }
        public string CustomerName { get; set; }
        public string PageTypeName { get; set; }
    }
}
