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
    
    public partial class BatchDetail
    {
        public long Id { get; set; }
        public long BatchId { get; set; }
        public long StatementId { get; set; }
        public long PageId { get; set; }
        public long WidgetId { get; set; }
        public string ImageURL { get; set; }
        public string VideoURL { get; set; }
        public string TenantCode { get; set; }
    }
}