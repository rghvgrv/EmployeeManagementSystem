using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Models.Entities;

public partial class EmployeeManagmentDbContext : DbContext
{
    public EmployeeManagmentDbContext()
    {
    }

    public EmployeeManagmentDbContext(DbContextOptions<EmployeeManagmentDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Authentication> Authentications { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\local;Database=EmployeeManagmentDB;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Authentication>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Authenti__3214EC07C24E7AC3");

            entity.ToTable("Authentication");

            entity.Property(e => e.AuthKey).HasMaxLength(512);
            entity.Property(e => e.LoginTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LogoutTime).HasColumnType("datetime");

            entity.HasOne(d => d.LoginUser).WithMany(p => p.Authentications)
                .HasForeignKey(d => d.LoginUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Authentic__Login__3E52440B");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmpId).HasName("PK__Employee__AF2DBB99F66E73E6");

            entity.ToTable("Employee");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Department).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Role).HasMaxLength(50);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.EmployeeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_Employee_CreatedBy");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.EmployeeModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK_Employee_ModifiedBy");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07DF5321A3");

            entity.ToTable("User");

            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Emp).WithMany(p => p.Users)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__EmpId__276EDEB3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
