//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TimeSheet.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblEmployee
    {
        public tblEmployee()
        {
            this.tblTimeSheets = new HashSet<tblTimeSheet>();
        }
    
        public System.Guid EmpID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime LatestModifiedTime { get; set; }
        public bool del { get; set; }
    
        public virtual ICollection<tblTimeSheet> tblTimeSheets { get; set; }
    }
}
