using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TimeSheet.Models
{
    [MetadataType(typeof(TimeSheetMetaData))]
    public partial class tblTimeSheet
    {
        public TimeSpan? TotalHours { get; set; }
        public class TimeSheetMetaData
        {
            [DisplayName("Discreption of Work")]
            public string TextBox1 { get; set; }

            [DisplayName("Total Hours")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:%d}d {0:%h}h {0:%m}m")]
            public TimeSpan? TotalHours { get; set; }
        }
    }
}