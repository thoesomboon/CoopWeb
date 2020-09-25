using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coop.Entities;

namespace Coop.Models.POCO
{
    public class ReasonModel
    {
        public int ReasonID { get; set; }
        public string ReasonName { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}