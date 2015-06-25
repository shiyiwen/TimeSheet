using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TimeSheet.Models
{
    [MetadataType(typeof(EmployeeMetaData))]
    public partial class tblEmployee
    {

        public class EmployeeMetaData
        {
            [Required]
            [DisplayName("First Name")]
            public string FirstName { get; set; }

            [Required]
            [DisplayName("Last Name")]
            public string LastName { get; set; }

            [Required]
            public string Email { get; set; }
        }
    }
}