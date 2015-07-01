using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Employee
    {
         public Employee()
        {
            this.TimeSheets = new HashSet<TimeSheet>();
        }
    
        public System.Guid EmpID { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime LatestModifiedTime { get; set; }
        public bool del { get; set; }
        public bool InOffice { get; set; }
    
        public virtual ICollection<TimeSheet> TimeSheets { get; set; }
    }
}
