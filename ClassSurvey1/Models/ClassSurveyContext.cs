﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ClassSurvey1.Models
{
    public partial class ClassSurveyContext : DbContext
    {
        public ClassSurveyContext()
        {
        }

        public ClassSurveyContext(DbContextOptions<ClassSurveyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Form> Forms { get; set; }
        public virtual DbSet<Lecturer> Lecturers { get; set; }
        public virtual DbSet<Operation> Operations { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentClass> StudentClasses { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<VersionSurvey> VersionSurveys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("data source=den1.mssql3.gear.host;initial catalog=ClassSurvey;persist security info=True;user id=classsurvey;password=Xh91~Qu4_1XO;multipleactiveresultsets=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admin");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.Vnumail)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Admin)
                    .HasForeignKey<Admin>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Admin_User");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ClassCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ClosedDate).HasColumnType("datetime");

                entity.Property(e => e.M).IsUnicode(false);

                entity.Property(e => e.M1).IsUnicode(false);

                entity.Property(e => e.OpenedDate).HasColumnType("datetime");

                entity.Property(e => e.Std).HasColumnName("STD");

                entity.Property(e => e.Std1).HasColumnName("STD1");

                entity.Property(e => e.Std2).HasColumnName("STD2");

                entity.Property(e => e.Subject)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.LecturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Class_Lecture");

                entity.HasOne(d => d.VersionSurvey)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.VersionSurveyId)
                    .HasConstraintName("FK_Class_VersionSurvey");
            });

            modelBuilder.Entity<Form>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Content).IsRequired();

                entity.HasOne(d => d.StudentClass)
                    .WithMany(p => p.Forms)
                    .HasForeignKey(d => d.StudentClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Form_StudentClass1");
            });

            modelBuilder.Entity<Lecturer>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.LecturerCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.Vnumail)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Lecturer)
                    .HasForeignKey<Lecturer>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lecturer_User");
            });

            modelBuilder.Entity<Operation>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Link)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Method)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Class)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Vnumail)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Student)
                    .HasForeignKey<Student>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Student_User");
            });

            modelBuilder.Entity<StudentClass>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.StudentClasses)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentClass_Class");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentClasses)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentClass_Student");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VersionSurvey>(entity =>
            {
                entity.ToTable("VersionSurvey");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });
        }
    }
}
