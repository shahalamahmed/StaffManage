using Microsoft.EntityFrameworkCore;
using StaffManage.Models;

namespace StaffManage.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; } // Changed from User to Employee

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data for Employee
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    Mobile = "+1234567890",
                    DateOfBirth = new DateTime(1985, 5, 15),
                    PhotoFile = "john_doe.jpg"
                },
                new Employee
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    Mobile = "+0987654321",
                    DateOfBirth = new DateTime(1990, 8, 25),
                    PhotoFile = "jane_smith.jpg"
                }
            );
        }
    }
}
