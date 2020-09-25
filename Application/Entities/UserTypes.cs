//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Coop.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserTypes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserTypes()
        {
            this.AccessPermissions = new HashSet<AccessPermissions>();
            this.Users = new HashSet<Users>();
        }
    
        public int UserTypeID { get; set; }
        public string UserTypeName { get; set; }
        public string UserTypeDescription { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ReserveCode1 { get; set; }
        public string ReserveCode2 { get; set; }
        public string ReserveCode3 { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccessPermissions> AccessPermissions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Users> Users { get; set; }
    }
}