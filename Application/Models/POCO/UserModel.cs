using Coop.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;
using System.Web;

namespace Coop.Models.POCO
{
    public class ViewUser
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string TypeName { get; set; }
        public bool IsActive { get; set; }
        public bool IsExpired { get; set; }
    }

    public class UserModel
    {
        public int UserID { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(Text))]
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(Text))]
        public string LastName { get; set; }

        //[Required]
        [StringLength(20, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "UserNameMaxLength")]
        [Display(Name = "UserName", ResourceType = typeof(Text))]
        public string UserName { get; set; }

        [StringLength(16, MinimumLength = 8, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "PasswordMaxLength")]
        [Display(Name = "Password", ResourceType = typeof(Text))]
        public string Password { get; set; }
       
        [Display(Name = "UserType", ResourceType = typeof(Text))]
        public int UserTypeID { get; set; }
        [Display(Name = "IsActive", ResourceType = typeof(Text))]
        public bool IsActive { get; set; }
       public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class OperationResult
    {
        public bool Result { get; set; }
        public string Message { get; set; }
    }
    public class UserResetModel
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        [RegularExpression(@"([a-z,A-Z,0-9 ]+\n*$)|(^[a-z,A-Z,0-9 ])",
        ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "AlphabetEnglishNumber")]
        [MinLength(8, ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = "PasswordMaxLength")]
        [Required(ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = "PasswordRequired")]
        [Remote("CheckValidUsernameAndPassword", "Account", AdditionalFields = "UserID,UserPassword")]       
        public string Password { get; set; }
    }
}