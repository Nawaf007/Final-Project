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
    
    public partial class Exam
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Exam()
        {
            this.SSExams = new HashSet<SSExam>();
        }
    
        public int Id { get; set; }
        public int SCId { get; set; }
        public int ExamType { get; set; }
        public System.DateTime ExamDate { get; set; }
        public int TotalMarks { get; set; }
        public int Weightage { get; set; }
    
        public virtual Lookup Lookup { get; set; }
        public virtual SectionCourse SectionCourse { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SSExam> SSExams { get; set; }
    }
}
