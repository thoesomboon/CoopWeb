using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop.Models.POCO
{
    [Serializable]
    public class ModuleCategoriesModel
    {
        public int ModuleCategoryID { get; set; }
        public string ModuleCategoryName { get; set; }
        public string ModuleCategoryDescription { get; set; }
        public string IconUrl { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}