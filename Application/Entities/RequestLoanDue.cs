//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Coop.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class RequestLoanDue
    {
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Filestatus { get; set; }
        public int CoopID { get; set; }
        public string RequestNo { get; set; }
        public System.DateTime DueDate { get; set; }
        public Nullable<decimal> LoanDueAmt { get; set; }
    
        public virtual RequestLoan RequestLoan { get; set; }
    }
}