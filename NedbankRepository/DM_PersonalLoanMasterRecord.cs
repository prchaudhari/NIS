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
    
    public partial class DM_PersonalLoanMasterRecord
    {
        public long Id { get; set; }
        public long BatchId { get; set; }
        public long CustomerId { get; set; }
        public long InvestorId { get; set; }
        public string Currency { get; set; }
        public string ProductType { get; set; }
        public string BranchId { get; set; }
        public string CreditAdvance { get; set; }
        public string OutstandingBalance { get; set; }
        public string AmountDue { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public string MonthlyInstallment { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public string Arrears { get; set; }
        public string AnnualRate { get; set; }
        public string Term { get; set; }
        public string TenantCode { get; set; }
    }
}
