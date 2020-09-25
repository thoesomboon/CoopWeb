using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coop.Infrastructure.Extensions;

namespace Coop.Models.POCO
{
    public class AccessPermissionModels
    {
        public int UserTypeID { get; set; }
        public int ModuleID { get; set; }
        public bool IsAccess { get; set; }
        public int CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class ViewAccessPermissionModels
    {
        public int UserTypeID { get; set; }
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
        public bool? IsAcive { get; set; }
        public int? SortOrder { get; set; }
        public int CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UserTypeName { get; set; }
    }

    public class GetUserType
    {
        public int UserTypeID { get; set; }
        public string UserTypeName { get; set; }     
    }
}