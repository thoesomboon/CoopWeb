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
    
    public partial class Title
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Title()
        {
            this.Member = new HashSet<Member>();
        }
    
        public int TitleID { get; set; }
        public string TitleName { get; set; }
        public string TitleNameEng { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Member> Member { get; set; }
    }
}
