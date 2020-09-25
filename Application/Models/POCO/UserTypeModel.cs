using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coop.Models.POCO
{

    public class UserTypeModels
    {
        public int UserTypeID { get; set; }
        public string UserTypeName { get; set; }
        public string UserTypeDescription { get; set; }
        public string UserTypeCode { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessageResourceType = typeof(ModelError), ErrorMessageResourceName = "TypePriorityNum"),
                 RegularExpression(@"(^\d*\.?\d*[1-9]+\d*$)|(^[1-9]+\d*\.\d*$)",
                 ErrorMessageResourceType = typeof(ModelError), ErrorMessageResourceName = "PriorityZero"),
                 Remote("CheckType", "EmployeeType", AdditionalFields = "EmployeeTypeID")]
        public int? TypePriority { get; set; }
        [Required(ErrorMessageResourceType = typeof(ModelError), ErrorMessageResourceName = "SortOrderRequired"),
         Remote("CheckSort", "EmployeeType", AdditionalFields = "EmployeeTypeID",
        ErrorMessageResourceType = typeof(ModelError), ErrorMessageResourceName = "SortOrderDuplicate")]
        public int? SortOrder { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string ReserveCode1 { get; set; }
        public string ReserveCode2 { get; set; }
        public string ReserveCode3 { get; set; }
        public bool IsSelect { get; set; }
    }
}