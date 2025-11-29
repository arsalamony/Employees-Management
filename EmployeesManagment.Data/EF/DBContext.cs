using EmployeesManagement.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesManagement.Data.EF
{
    public class DBContext : DbContext
    {

        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(clsDataAccessSettings.ConnectionString);
        }



        // Tables
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<SystemRecord> SystemRecords { get; set; }

        public DbSet<SalaryRate> SalaryRates { get; set; }


        public DbSet<Employee> Employees { get; set; }

        public DbSet<BookThank> BookThanks { get; set; }

        public DbSet<EmployeeRecord> EmployeeRecords { get; set; }

    }
}
