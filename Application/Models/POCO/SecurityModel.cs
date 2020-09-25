using System;
using Coop.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coop.Models.POCO
{
    public class SecurityModel
    {
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Filestatus { get; set; }
        public int CoopID { get; set; }
        public string LoanID { get; set; }
        public int Seq { get; set; }
        public Nullable<int> SecurityTypeID { get; set; }
        public string SecurityTypeName { get; set; }
        public string LicenceNo { get; set; }
        public string OwnerName { get; set; }
        public string Area { get; set; }
        public Nullable<decimal> Value { get; set; }
    }
}
