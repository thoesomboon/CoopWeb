using System;
using Coop.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Coop.Models.POCO
{
    public class MemberModel
    {
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public  Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Filestatus { get; set; }
        public int CoopID { get; set; }
        public string MemberID { get; set; }
        public Nullable<int> TitleID { get; set; }
        public string TitleName { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Nullable<int> SubDistrictID { get; set; }
        public String SubDistrictName { get; set; }
        public Nullable<int> DistrictID { get; set; }
        public String DistrictName { get; set; }
        public Nullable<int> ProvinceID { get; set; }
        public String ProvinceName { get; set; }
        public string PostalCode { get; set; }
        [RegularExpression(@"^(0[2,3,4,5,6,7])\d{1}-\d{3}-\d{3}|(0[2,3,4,5,6,7])\d{9}", ErrorMessage = "เบอร์โทรศัพท์บ้านไม่ถูกต้อง")]
        public string Telephone { get; set; }
        [RegularExpression(@"^(0[2,3,4,5,6,7])\d{1}-\d{3}-\d{3}|(0[2,3,4,5,6,7])\d{10}", ErrorMessage = "เบอร์มือถือไม่ถูกต้อง")]
        public string Mobile { get; set; }
        public string EMail { get; set; }
        public string IdCard { get; set; }
        public string LineID { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public String BirthDateTH { get; set; }
        public Nullable<System.DateTime> ApplyDate { get; set; }
        public String ApplyDateTH { get; set; }
        public Nullable<System.DateTime> ResignDate { get; set; }
        public String ResignDateTH { get; set; }
        public Nullable<decimal> Salary { get; set; }
        public Nullable<int> MemberGroupID { get; set; }
        public String MemberGroupName { get; set; }
        public Nullable<int> MemberTypeID { get; set; }
        public String MemberTypeName { get; set; }

    }
    public enum Result
    {
        Unknown = 0,
        Success = 1,
        Unsuccess = 2
    }
}
