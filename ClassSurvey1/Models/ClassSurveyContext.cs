using System;
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
        public virtual DbSet<Clas> Classes { get; set; }
        public virtual DbSet<Lecturer> Lecturers { get; set; }
        public virtual DbSet<Operation> Operations { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentClas> StudentClasses { get; set; }
        public virtual DbSet<Survey> Surveys { get; set; }
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

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Vnumail)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Clas>(entity =>
            {
                entity.ToTable("Class");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ClassCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.M).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.M1).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.M2).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.Std)
                    .HasColumnName("STD")
                    .HasColumnType("decimal(10, 0)");

                entity.Property(e => e.Std1)
                    .HasColumnName("STD1")
                    .HasColumnType("decimal(10, 0)");

                entity.Property(e => e.Std2)
                    .HasColumnName("STD2")
                    .HasColumnType("decimal(10, 0)");

                entity.Property(e => e.Subject)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Lecture)
                    .WithMany(p => p.Clas)
                    .HasForeignKey(d => d.LectureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Class_Lecture");
            });

            modelBuilder.Entity<Lecturer>(entity =>
            {
                entity.ToTable("Lecturer");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.LectureCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Vnumail)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Operation>(entity =>
            {
                entity.ToTable("Operation");

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
                entity.ToTable("Student");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Vnumail)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StudentClas>(entity =>
            {
                entity.ToTable("StudentClass");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.StudentClas)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentClass_Class");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentClas)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentClass_Student");
            });

            modelBuilder.Entity<Survey>(entity =>
            {
                entity.ToTable("Survey");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Content).IsRequired();

                entity.HasOne(d => d.StudentClass)
                    .WithMany(p => p.Surveys)
                    .HasForeignKey(d => d.StudentClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Suvery_StudentClass1");

                entity.HasOne(d => d.VersionSurvey)
                    .WithMany(p => p.Surveys)
                    .HasForeignKey(d => d.VersionSurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Suvery_VersionSurvey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Admin");

                entity.HasOne(d => d.Id1)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Lecture");

                entity.HasOne(d => d.Id2)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Student");
            });

            modelBuilder.Entity<VersionSurvey>(entity =>
            {
                entity.ToTable("VersionSurvey");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });
        }
    }
}
