using System;
using Coop.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coop.Models.POCO
{
    public class InterestModel
    {
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Filestatus { get; set; }
        public int CoopID { get; set; }
        public string Type { get; set; }
        public string TInt { get; set; }
        public System.DateTime FirstEffectDate { get; set; }
        public Nullable<System.DateTime> LastEffectDate { get; set; }
        public Nullable<decimal> Balance1 { get; set; }
        public Nullable<decimal> Rate1 { get; set; }
        public Nullable<decimal> ChargeRate1 { get; set; }
        public Nullable<decimal> Balance2 { get; set; }
        public Nullable<decimal> Rate2 { get; set; }
        public Nullable<decimal> ChargeRate2 { get; set; }
        public Nullable<decimal> Balance3 { get; set; }
        public Nullable<decimal> Rate3 { get; set; }
        public Nullable<decimal> ChargeRate3 { get; set; }
        public Nullable<decimal> Balance4 { get; set; }
        public Nullable<decimal> Rate4 { get; set; }
        public Nullable<decimal> ChargeRate4 { get; set; }
        public Nullable<decimal> Balance5 { get; set; }
        public Nullable<decimal> Rate5 { get; set; }
        public Nullable<decimal> ChargeRate5 { get; set; }
    }
}
