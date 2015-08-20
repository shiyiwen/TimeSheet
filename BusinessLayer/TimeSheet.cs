using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class TimeSheet
    {
        public System.Guid ID { get; set; }
        public System.Guid EmpID { get; set; }
        [DisplayName("In Time")]
        public Nullable<System.DateTime> InTime { get; set; }
        [DisplayName("Out Time")]
        public Nullable<System.DateTime> OutTime { get; set; }
        [DisplayName("Total Hours")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:%d}d {0:%h}h {0:%m}m")]
        public TimeSpan? TotalHours { get; set; }
        [DisplayName("Reason for Out")]
        public string TextBox1 { get; set; }
        public string TextBox2 { get; set; }
        public string TextBox3 { get; set; }

        public Employee employee { get; set; }
    }
}
