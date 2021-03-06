﻿using System;
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
        
        public IEnumerable<TimeSheet> ALLTimeSheets
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

        public IEnumerable<TimeSheet> TimeSheetsByNameorInTimeorOutTime(Guid? EmpID, Boolean? Del, DateTime? StartInTime, DateTime? StopInTime, DateTime? StartOutTime, DateTime? StopOutTime)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TSDB"].ConnectionString;

            List<TimeSheet> timesheets = new List<TimeSheet>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetTimeSheetByNameorInTimeorOutTime", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (EmpID != null)
                    cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = EmpID;
                else
                    cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = DBNull.Value;
                if (Del != null)
                    cmd.Parameters.Add("@Del", SqlDbType.Bit).Value = Del;
                else
                    cmd.Parameters.Add("@Del", SqlDbType.Bit).Value = DBNull.Value;
                if (StartInTime != null)
                    cmd.Parameters.Add("@StartInTime", SqlDbType.DateTime).Value = StartInTime;
                else
                    cmd.Parameters.Add("@StartInTime", SqlDbType.DateTime).Value = DBNull.Value;
                if (StopInTime != null)
                    cmd.Parameters.Add("@EndInTime", SqlDbType.DateTime).Value = StopInTime;
                else
                    cmd.Parameters.Add("@EndInTime", SqlDbType.DateTime).Value = DBNull.Value;
                if (StartOutTime != null)
                    cmd.Parameters.Add("@StartOutTime", SqlDbType.DateTime).Value = StartOutTime;
                else
                    cmd.Parameters.Add("@StartOutTime", SqlDbType.DateTime).Value = DBNull.Value;
                if (StopOutTime != null)
                    cmd.Parameters.Add("@EndOutTime", SqlDbType.DateTime).Value = StopOutTime;
                else
                    cmd.Parameters.Add("@EndOutTime", SqlDbType.DateTime).Value = DBNull.Value;
                
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
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

        public int CountInTime(Guid? empid, Boolean? del, int year, int? month, int startHour, int startMinute, int stopHour, int stopMinute)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TSDB"].ConnectionString;

            List<TimeSheet> timesheets = new List<TimeSheet>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetCountofInTime", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (empid != null)
                    cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = empid;
                else
                    cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = DBNull.Value;
                if (del != null)
                    cmd.Parameters.Add("@Del", SqlDbType.Bit).Value = del;
                else
                    cmd.Parameters.Add("@Del", SqlDbType.Bit).Value = DBNull.Value;
                cmd.Parameters.Add("@Year", SqlDbType.Int).Value = year;
                if (month != null)
                    cmd.Parameters.Add("@Month", SqlDbType.Int).Value = month;
                else
                    cmd.Parameters.Add("@Month", SqlDbType.Int).Value = DBNull.Value;
                cmd.Parameters.Add("@StartHour", SqlDbType.Int).Value = startHour;
                cmd.Parameters.Add("@StartMinute", SqlDbType.Int).Value = startMinute;
                cmd.Parameters.Add("@EndHour", SqlDbType.Int).Value = stopHour;
                cmd.Parameters.Add("@EndMinute", SqlDbType.Int).Value = stopMinute;
                cmd.Parameters.Add("@TimesheetCount", SqlDbType.Int).Direction=ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();

                int Count = int.Parse(cmd.Parameters["@TimesheetCount"].Value.ToString());

                con.Close();
                return Count;
            }
        }

        public int CountOutTime(Guid? empid, Boolean? del, int year, int? month, int startHour, int startMinute, int stopHour, int stopMinute)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TSDB"].ConnectionString;

            List<TimeSheet> timesheets = new List<TimeSheet>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetCountofOutTime", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (empid != null)
                    cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = empid;
                else
                    cmd.Parameters.Add("@EmpID", SqlDbType.UniqueIdentifier).Value = DBNull.Value;
                if (del != null)
                    cmd.Parameters.Add("@Del", SqlDbType.Bit).Value = del;
                else
                    cmd.Parameters.Add("@Del", SqlDbType.Bit).Value = DBNull.Value;
                cmd.Parameters.Add("@Year", SqlDbType.Int).Value = year;
                if (month != null)
                    cmd.Parameters.Add("@Month", SqlDbType.Int).Value = month;
                else
                    cmd.Parameters.Add("@Month", SqlDbType.Int).Value = DBNull.Value;
                cmd.Parameters.Add("@StartHour", SqlDbType.Int).Value = startHour;
                cmd.Parameters.Add("@StartMinute", SqlDbType.Int).Value = startMinute;
                cmd.Parameters.Add("@EndHour", SqlDbType.Int).Value = stopHour;
                cmd.Parameters.Add("@EndMinute", SqlDbType.Int).Value = stopMinute;
                cmd.Parameters.Add("@TimesheetCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();

                int Count = int.Parse(cmd.Parameters["@TimesheetCount"].Value.ToString());

                con.Close();
                return Count;
            }
        }
    }
}
