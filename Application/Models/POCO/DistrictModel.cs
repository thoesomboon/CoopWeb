using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop.Models.POCO
{
    public class DistrictModel
    {
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
        public string DistrictCode { get; set; }
        public bool IsActive { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
    }
}
