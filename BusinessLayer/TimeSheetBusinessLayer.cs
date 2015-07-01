using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration; 
using System.Data.SqlClient;
using System.Data;

namespace BusinessLayer
{
    public class TimeSheetBusinessLayer
    {
        
        public IEnumerable<TimeSheet> TimeSheets
        {
            get
            {
                string connectionString = ConfigurationManager.ConnectionStrings["TSDB"].ConnectionString;

                List<TimeSheet> timesheets = new List<TimeSheet>();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spGetAllTimeSheet", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read()) {
                        TimeSheet timesheet = new TimeSheet();
                        timesheet.employee = new Employee();
                        timesheet.employee.FirstName = dr["FirstName"].ToString();
                        timesheet.employee.LastName = dr["LastName"].ToString();
                        if (!dr.IsDBNull(dr.GetOrdinal("InTime")))
                        {
                            timesheet.InTime = Convert.ToDateTime(dr["InTime"]);
                        }
                        //timesheet.InTime = dr["InTime"] as DateTime;
                        if (!dr.IsDBNull(dr.GetOrdinal("OutTime")))
                        {
                            timesheet.OutTime = Convert.ToDateTime(dr["OutTime"]);
                            timesheet.TotalHours = TimeSpan.FromMinutes(System.Convert.ToDouble(dr["TotalHours"]));
                        }
                        timesheet.TextBox1 = dr["TextBox1"].ToString();
                        timesheets.Add(timesheet);
                    }
                    con.Close();
                }
                return timesheets;    
            }       
        }
    }
}
