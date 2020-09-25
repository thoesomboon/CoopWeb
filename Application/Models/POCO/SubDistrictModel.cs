using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop.Models.POCO
{
    public class SubDistrictModel
    {
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int SubDistrictID { get; set; }
        public string SubDistrictName { get; set; }
        public string SubDistrictDescription { get; set; }
        public string SubDistrictCode { get; set; }
        public string PostalCode { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}