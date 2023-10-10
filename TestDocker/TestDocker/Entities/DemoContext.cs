using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TestDocker.Entities;

public partial class DemoContext : IdentityDbContext
{
    public DemoContext()
    {
    }
    public DemoContext(DbContextOptions<DemoContext> options)
        : base(options)
    {
    }
    public virtual DbSet<CurrentDeptEmp> CurrentDeptEmps { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DeptEmp> DeptEmps { get; set; }

    public virtual DbSet<DeptEmpLatestDate> DeptEmpLatestDates { get; set; }

    public virtual DbSet<DeptManager> DeptManagers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Salary> Salaries { get; set; }

    public virtual DbSet<Title> Titles { get; set; }
    public virtual DbSet<UserApp> UserApp { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseMySQL("server=localhost;port=3307;user=root;password=Cong@@0703;database=test");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region : config
        modelBuilder.Entity<CurrentDeptEmp>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("current_dept_emp");

            entity.Property(e => e.DeptNo)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("dept_no");
            entity.Property(e => e.EmpNo).HasColumnName("emp_no");
            entity.Property(e => e.FromDate)
                .HasColumnType("date")
                .HasColumnName("from_date");
            entity.Property(e => e.ToDate)
                .HasColumnType("date")
                .HasColumnName("to_date");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DeptNo).HasName("PRIMARY");

            entity.ToTable("departments");

            entity.HasIndex(e => e.DeptName, "dept_name").IsUnique();

            entity.Property(e => e.DeptNo)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("dept_no");
            entity.Property(e => e.DeptName)
                .HasMaxLength(40)
                .HasColumnName("dept_name");
        });

        modelBuilder.Entity<DeptEmp>(entity =>
        {
            entity.HasKey(e => new { e.EmpNo, e.DeptNo }).HasName("PRIMARY");

            entity.ToTable("dept_emp");

            entity.HasIndex(e => e.DeptNo, "dept_no");

            entity.Property(e => e.EmpNo).HasColumnName("emp_no");
            entity.Property(e => e.DeptNo)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("dept_no");
            entity.Property(e => e.FromDate)
                .HasColumnType("date")
                .HasColumnName("from_date");
            entity.Property(e => e.ToDate)
                .HasColumnType("date")
                .HasColumnName("to_date");

            entity.HasOne(d => d.DeptNoNavigation).WithMany(p => p.DeptEmps)
                .HasForeignKey(d => d.DeptNo)
                .HasConstraintName("dept_emp_ibfk_2");

            entity.HasOne(d => d.EmpNoNavigation).WithMany(p => p.DeptEmps)
                .HasForeignKey(d => d.EmpNo)
                .HasConstraintName("dept_emp_ibfk_1");
        });

        modelBuilder.Entity<DeptEmpLatestDate>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("dept_emp_latest_date");

            entity.Property(e => e.EmpNo).HasColumnName("emp_no");
            entity.Property(e => e.FromDate)
                .HasColumnType("date")
                .HasColumnName("from_date");
            entity.Property(e => e.ToDate)
                .HasColumnType("date")
                .HasColumnName("to_date");
        });

        modelBuilder.Entity<DeptManager>(entity =>
        {
            entity.HasKey(e => new { e.EmpNo, e.DeptNo }).HasName("PRIMARY");

            entity.ToTable("dept_manager");

            entity.HasIndex(e => e.DeptNo, "dept_no");

            entity.Property(e => e.EmpNo).HasColumnName("emp_no");
            entity.Property(e => e.DeptNo)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("dept_no");
            entity.Property(e => e.FromDate)
                .HasColumnType("date")
                .HasColumnName("from_date");
            entity.Property(e => e.ToDate)
                .HasColumnType("date")
                .HasColumnName("to_date");

            entity.HasOne(d => d.DeptNoNavigation).WithMany(p => p.DeptManagers)
                .HasForeignKey(d => d.DeptNo)
                .HasConstraintName("dept_manager_ibfk_2");

            entity.HasOne(d => d.EmpNoNavigation).WithMany(p => p.DeptManagers)
                .HasForeignKey(d => d.EmpNo)
                .HasConstraintName("dept_manager_ibfk_1");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmpNo).HasName("PRIMARY");

            entity.ToTable("employees");

            entity.Property(e => e.EmpNo).HasColumnName("emp_no");
            entity.Property(e => e.BirthDate)
                .HasColumnType("date")
                .HasColumnName("birth_date");
            entity.Property(e => e.FirstName)
                .HasMaxLength(14)
                .HasColumnName("first_name");
            entity.Property(e => e.Gender)
                .HasColumnType("enum('M','F')")
                .HasColumnName("gender");
            entity.Property(e => e.HireDate)
                .HasColumnType("date")
                .HasColumnName("hire_date");
            entity.Property(e => e.LastName)
                .HasMaxLength(16)
                .HasColumnName("last_name");
        });

        modelBuilder.Entity<Salary>(entity =>
        {
            entity.HasKey(e => new { e.EmpNo, e.FromDate }).HasName("PRIMARY");

            entity.ToTable("salaries");

            entity.Property(e => e.EmpNo).HasColumnName("emp_no");
            entity.Property(e => e.FromDate)
                .HasColumnType("date")
                .HasColumnName("from_date");
            entity.Property(e => e.Salary1).HasColumnName("salary");
            entity.Property(e => e.ToDate)
                .HasColumnType("date")
                .HasColumnName("to_date");

            entity.HasOne(d => d.EmpNoNavigation).WithMany(p => p.Salaries)
                .HasForeignKey(d => d.EmpNo)
                .HasConstraintName("salaries_ibfk_1");
        });

        modelBuilder.Entity<Title>(entity =>
        {
            entity.HasKey(e => new { e.EmpNo, e.Title1, e.FromDate }).HasName("PRIMARY");

            entity.ToTable("titles");

            entity.Property(e => e.EmpNo).HasColumnName("emp_no");
            entity.Property(e => e.Title1)
                .HasMaxLength(50)
                .HasColumnName("title");
            entity.Property(e => e.FromDate)
                .HasColumnType("date")
                .HasColumnName("from_date");
            entity.Property(e => e.ToDate)
                .HasColumnType("date")
                .HasColumnName("to_date");

            entity.HasOne(d => d.EmpNoNavigation).WithMany(p => p.Titles)
                .HasForeignKey(d => d.EmpNo)
                .HasConstraintName("titles_ibfk_1");
        });
        #endregion
        base.OnModelCreating(modelBuilder);
    }
}
