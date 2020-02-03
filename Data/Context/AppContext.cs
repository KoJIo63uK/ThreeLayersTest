using System;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class AppContext : DbContext
    {
        public virtual DbSet<Employee> Employees { get; set; }

        public AppContext() {}
        
        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData
            (
                new Employee
                {
                    Id = Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936023"),
                    Name = "Vlad",
                    Surname = "Vorosalov"
                },
                new Employee
                {
                    Id = Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936024"),
                    Name = "Olya",
                    Surname = "Vorosalova"
                    
                });

            modelBuilder.Entity<Employee>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
        }
    }
}