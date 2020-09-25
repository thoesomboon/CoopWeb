using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop.Models.POCO
{
    public class ProvinceModel
    {
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public int ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        public string ProvinceDescription { get; set; }
        public string ProvinceCode { get; set; }
    }
    public class ProvinceListModel
    {
        public int ProvinceID { get; set; }
        public string ProvinceName { get; set; }
    }
}