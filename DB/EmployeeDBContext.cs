using EmployeeManagementSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.DB
{
    public class EmployeeDBContext : DbContext
    {
        public EmployeeDBContext(DbContextOptions<EmployeeDBContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Authentication> Authentication { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                // Explicitly define the primary key for the Employee entity
                entity.HasKey(e => e.EmpId);

                // Additional configuration if necessary
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.Role).HasMaxLength(50);
                entity.Property(e => e.Department).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {

                // Explicitly define the primary key for the User entity
                entity.HasKey(e => e.Id);

                // Additional configuration if necessary
                entity.Property(e => e.Username).HasMaxLength(50);
                entity.Property(e => e.PasswordHash).HasMaxLength(50);
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
