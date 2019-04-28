//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StudentManagmentSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SectionStudent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SectionStudent()
        {
            this.SSAssignments = new HashSet<SSAssignment>();
            this.SSExams = new HashSet<SSExam>();
        }
    
        public int Id { get; set; }
        public int SectionId { get; set; }
        public string StudentId { get; set; }
    
        public virtual Section Section { get; set; }
        public virtual Student Student { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SSAssignment> SSAssignments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SSExam> SSExams { get; set; }
    }
}