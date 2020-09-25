using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coop.Entities;

namespace Coop.Models.POCO
{
    public class TitleModel
    {
        public int TitleID { get; set; }
        public string TitleName { get; set; }
        public string TitleNameEng { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
    public class TitleListModel
    {
        public int TitleID { get; set; }
        public string TitleName { get; set; }
    }
}