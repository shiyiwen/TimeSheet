﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class TimeSheetContext : DbContext
    {
        public TimeSheetContext()
            : base("name=TimeSheetContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<tblEmployee> tblEmployees { get; set; }
        public DbSet<tblTimeSheet> tblTimeSheets { get; set; }
    
        public virtual ObjectResult<spGetAllEmployee_Result> spGetAllEmployee()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetAllEmployee_Result>("spGetAllEmployee");
        }
    }
}
