using System;
using System.Collections.Generic;
using ExamSession.Tables;
using Microsoft.EntityFrameworkCore;

namespace ExamSession;

public partial class ExamSessionContext : DbContext
{
    public ExamSessionContext()
    {
    }

    public ExamSessionContext(DbContextOptions<ExamSessionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Assessment> Assessments { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudyGroup> StudyGroups { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<TeacherItem> TeacherItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=ExamSession;User ID=postgres; Password=503992");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assessment>(entity =>
        {
            entity.HasKey(e => e.AssessmentId).HasName("assessment_pkey");

            entity.ToTable("assessment");

            entity.Property(e => e.AssessmentId).HasColumnName("assessment_id");
            entity.Property(e => e.ExamDate).HasColumnName("exam_date");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.StudentId).HasColumnName("student_id");

            entity.HasOne(d => d.Item).WithMany(p => p.Assessments)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("assessment_item_id_fkey");

            entity.HasOne(d => d.Student).WithMany(p => p.Assessments)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("assessment_student_id_fkey");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("department_pkey");

            entity.ToTable("department");

            entity.HasIndex(e => e.HeadOfTheDepartment, "department_head_of_the_department_key").IsUnique();

            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.AudienceNumber).HasColumnName("audience_number");
            entity.Property(e => e.Email)
                .HasMaxLength(20)
                .HasColumnName("email");
            entity.Property(e => e.HeadOfTheDepartment).HasColumnName("head_of_the_department");
            entity.Property(e => e.Name)
                .HasMaxLength(35)
                .HasColumnName("name");
            entity.Property(e => e.NumberOfEmployees).HasColumnName("number_of_employees");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(16)
                .HasColumnName("phone_number");

            entity.HasOne(d => d.HeadOfTheDepartmentNavigation).WithOne(p => p.Department)
                .HasForeignKey<Department>(d => d.HeadOfTheDepartment)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("department_head_of_the_department_fkey");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("item_pkey");

            entity.ToTable("item");

            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.HoursNum).HasColumnName("hours_num");
            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("student_pkey");

            entity.ToTable("student");

            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Email)
                .HasMaxLength(20)
                .HasColumnName("email");
            entity.Property(e => e.LastName)
                .HasMaxLength(15)
                .HasColumnName("last_name");
            entity.Property(e => e.Name)
                .HasMaxLength(15)
                .HasColumnName("name");
            entity.Property(e => e.NumberGroup)
                .HasMaxLength(10)
                .HasColumnName("number_group");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(16)
                .HasColumnName("phone_number");

            entity.HasOne(d => d.NumberGroupNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.NumberGroup)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("student_number_group_fkey");
        });

        modelBuilder.Entity<StudyGroup>(entity =>
        {
            entity.HasKey(e => e.NumberGroup).HasName("study_group_pkey");

            entity.ToTable("study_group");

            entity.Property(e => e.NumberGroup)
                .HasMaxLength(10)
                .HasColumnName("number_group");
            entity.Property(e => e.Elder).HasColumnName("elder");
            entity.Property(e => e.NumberOfStudents).HasColumnName("number_of_students");
            entity.Property(e => e.Specialization)
                .HasMaxLength(30)
                .HasColumnName("specialization");
            entity.Property(e => e.Tutor).HasColumnName("tutor");

            entity.HasOne(d => d.ElderNavigation).WithMany(p => p.StudyGroups)
                .HasForeignKey(d => d.Elder)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("study_group_elder_fkey");

            entity.HasOne(d => d.TutorNavigation).WithMany(p => p.StudyGroups)
                .HasForeignKey(d => d.Tutor)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("study_group_tutor_fkey");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("teacher_pkey");

            entity.ToTable("teacher");

            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Email)
                .HasMaxLength(40)
                .HasColumnName("email");
            entity.Property(e => e.LastName)
                .HasMaxLength(15)
                .HasColumnName("last_name");
            entity.Property(e => e.Name)
                .HasMaxLength(15)
                .HasColumnName("name");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(15)
                .HasColumnName("patronymic");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(16)
                .HasColumnName("phone_number");
        });

        modelBuilder.Entity<TeacherItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("teacher_item_pkey");

            entity.ToTable("teacher_item");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

            entity.HasOne(d => d.Item).WithMany(p => p.TeacherItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("teacher_item_item_id_fkey");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TeacherItems)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("teacher_item_teacher_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
