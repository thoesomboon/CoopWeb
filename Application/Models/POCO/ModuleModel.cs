using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop.Models.POCO
{
    [Serializable]
    public class ModuleModel
    {
        public int ModuleID { get; set; }
        public int ModuleCategoryID { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
        public string ModuleURL { get; set; }
        public string IconUrl { get; set; }
        public bool IsActive { get; set; }
        public int? SortOrder { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        //public int? ParentModuleID { get; set; }
        //public string ReserveCode1 { get; set; }
        //public bool LineType { get; set; }
        public int CountSubMenu { get; set; }
        public string ModuleCategoryName { get; set; }
    }
 }