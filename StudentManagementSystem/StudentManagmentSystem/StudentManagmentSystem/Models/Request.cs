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
    
    public partial class Request
    {
        public string AspId { get; set; }
        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public string Description { get; set; }
        public int Id { get; set; }
        public string Topic { get; set; }
        public string Acknowledged { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual AspNetUser AspNetUser1 { get; set; }
    }
}
