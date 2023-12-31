﻿#nullable disable
using Microsoft.EntityFrameworkCore;
using EFCore7UnableDoMigration.Domain.Entities;

namespace EFCore7UnableDoMigration.Domain.Data;

public partial class ContosoUniversityContext : DbContext
{
    public ContosoUniversityContext(DbContextOptions<ContosoUniversityContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Course { get; set; }

    public virtual DbSet<Department> Department { get; set; }

    public virtual DbSet<Enrollment> Enrollment { get; set; }

    public virtual DbSet<OfficeAssignment> OfficeAssignment { get; set; }

    public virtual DbSet<Person> Person { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK_dbo.Course");

            entity.HasIndex(e => e.DepartmentId, "IX_DepartmentID");

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.DepartmentId)
                .HasDefaultValueSql("((1))")
                .HasColumnName("DepartmentID");
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Department).WithMany(p => p.Course)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_dbo.Course_dbo.Department_DepartmentID");

            entity.HasMany(d => d.Instructor).WithMany(p => p.Course)
                .UsingEntity<Dictionary<string, object>>(
                    "CourseInstructor",
                    r => r.HasOne<Person>().WithMany()
                        .HasForeignKey("InstructorId")
                        .HasConstraintName("FK_dbo.CourseInstructor_dbo.Instructor_InstructorID"),
                    l => l.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId")
                        .HasConstraintName("FK_dbo.CourseInstructor_dbo.Course_CourseID"),
                    j =>
                    {
                        j.HasKey("CourseId", "InstructorId").HasName("PK_dbo.CourseInstructor");
                        j.HasIndex(new[] { "CourseId" }, "IX_CourseID");
                        j.HasIndex(new[] { "InstructorId" }, "IX_InstructorID");
                        j.IndexerProperty<int>("CourseId").HasColumnName("CourseID");
                        j.IndexerProperty<int>("InstructorId").HasColumnName("InstructorID");
                    });
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK_dbo.Department");

            entity.HasIndex(e => e.InstructorId, "IX_InstructorID");

            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.Budget).HasColumnType("money");
            entity.Property(e => e.InstructorId).HasColumnName("InstructorID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.RowVersion)
                .IsRequired()
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.Instructor).WithMany(p => p.Department)
                .HasForeignKey(d => d.InstructorId)
                .HasConstraintName("FK_dbo.Department_dbo.Instructor_InstructorID");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("PK_dbo.Enrollment");

            entity.HasIndex(e => e.CourseId, "IX_CourseID");

            entity.HasIndex(e => e.StudentId, "IX_StudentID");

            entity.Property(e => e.EnrollmentId).HasColumnName("EnrollmentID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Course).WithMany(p => p.Enrollment)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_dbo.Enrollment_dbo.Course_CourseID");

            entity.HasOne(d => d.Student).WithMany(p => p.Enrollment)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_dbo.Enrollment_dbo.Person_StudentID");
        });

        modelBuilder.Entity<OfficeAssignment>(entity =>
        {
            entity.HasKey(e => e.InstructorId).HasName("PK_dbo.OfficeAssignment");

            entity.HasIndex(e => e.InstructorId, "IX_InstructorID");

            entity.Property(e => e.InstructorId)
                .ValueGeneratedNever()
                .HasColumnName("InstructorID");
            entity.Property(e => e.Location).HasMaxLength(50);

            entity.HasOne(d => d.Instructor).WithOne(p => p.OfficeAssignment)
                .HasForeignKey<OfficeAssignment>(d => d.InstructorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.OfficeAssignment_dbo.Instructor_InstructorID");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Person");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Discriminator)
                .IsRequired()
                .HasMaxLength(128)
                .HasDefaultValueSql("('Instructor')");
            entity.Property(e => e.EnrollmentDate).HasColumnType("datetime");
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.HireDate).HasColumnType("datetime");
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}