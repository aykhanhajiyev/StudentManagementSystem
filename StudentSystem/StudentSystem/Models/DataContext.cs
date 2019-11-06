using Microsoft.AspNet.Identity.EntityFramework;
using StudentSystem.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace StudentSystem.Models
{
    public class DataContext:DbContext
    {
        public DataContext():base("dataConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<IdentityDataContext, StudentSystem.Migrations.Configuration>());
        }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Attendances> Attendances { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamUserResult> ExamUserResults { get; set; }

        public DbSet<Anouncements> Anouncements { get; set; }
        
    }
}