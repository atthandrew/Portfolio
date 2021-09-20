using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LMS.Models.LMSModels
{
    public partial class Team83LMSContext : DbContext
    {
        public Team83LMSContext()
        {
        }

        public Team83LMSContext(DbContextOptions<Team83LMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Administrators> Administrators { get; set; }
        public virtual DbSet<AssignmentCategories> AssignmentCategories { get; set; }
        public virtual DbSet<Assignments> Assignments { get; set; }
        public virtual DbSet<Classes> Classes { get; set; }
        public virtual DbSet<Courses> Courses { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<Enrollments> Enrollments { get; set; }
        public virtual DbSet<Professors> Professors { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<Submissions> Submissions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("Server=atr.eng.utah.edu;User Id=u0956834;Password=roger1234;Database=Team83LMS");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrators>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("char(8)");

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.FName)
                    .IsRequired()
                    .HasColumnName("fName")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.LName)
                    .IsRequired()
                    .HasColumnName("lName")
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<AssignmentCategories>(entity =>
            {
                entity.HasKey(e => e.AcId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => new { e.ClassId, e.AcName })
                    .HasName("classID")
                    .IsUnique();

                entity.Property(e => e.AcId).HasColumnName("acID");

                entity.Property(e => e.AcName)
                    .IsRequired()
                    .HasColumnName("acName")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.ClassId).HasColumnName("classID");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.AssignmentCategories)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("AssignmentCategories_ibfk_1");
            });

            modelBuilder.Entity<Assignments>(entity =>
            {
                entity.HasKey(e => e.AId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.AcId)
                    .HasName("acID");

                entity.HasIndex(e => new { e.Name, e.AcId })
                    .HasName("name")
                    .IsUnique();

                entity.Property(e => e.AId).HasColumnName("aID");

                entity.Property(e => e.AContents)
                    .IsRequired()
                    .HasColumnName("aContents")
                    .HasColumnType("varchar(8192)");

                entity.Property(e => e.AcId).HasColumnName("acID");

                entity.Property(e => e.DueDate)
                    .HasColumnName("dueDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.MaxPoints).HasColumnName("maxPoints");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.Ac)
                    .WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.AcId)
                    .HasConstraintName("Assignments_ibfk_1");
            });

            modelBuilder.Entity<Classes>(entity =>
            {
                entity.HasKey(e => e.ClassId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.CourseId)
                    .HasName("courseID");

                entity.HasIndex(e => e.ProfId)
                    .HasName("profID");

                entity.HasIndex(e => new { e.SemSeason, e.SemYear, e.CourseId })
                    .HasName("semSeason")
                    .IsUnique();

                entity.Property(e => e.ClassId).HasColumnName("classID");

                entity.Property(e => e.CourseId).HasColumnName("courseID");

                entity.Property(e => e.ETime)
                    .HasColumnName("eTime")
                    .HasColumnType("time");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasColumnName("location")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.ProfId)
                    .IsRequired()
                    .HasColumnName("profID")
                    .HasColumnType("char(8)");

                entity.Property(e => e.STime)
                    .HasColumnName("sTime")
                    .HasColumnType("time");

                entity.Property(e => e.SemSeason)
                    .IsRequired()
                    .HasColumnName("semSeason")
                    .HasColumnType("varchar(6)");

                entity.Property(e => e.SemYear).HasColumnName("semYear");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Classes_ibfk_1");

                entity.HasOne(d => d.Prof)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.ProfId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Classes_ibfk_2");
            });

            modelBuilder.Entity<Courses>(entity =>
            {
                entity.HasKey(e => e.CourseId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.DSubject)
                    .HasName("dSubject");

                entity.HasIndex(e => new { e.CNumber, e.DSubject })
                    .HasName("cNumber")
                    .IsUnique();

                entity.Property(e => e.CourseId).HasColumnName("courseID");

                entity.Property(e => e.CName)
                    .IsRequired()
                    .HasColumnName("cName")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.CNumber).HasColumnName("cNumber");

                entity.Property(e => e.DSubject)
                    .IsRequired()
                    .HasColumnName("dSubject")
                    .HasColumnType("varchar(4)");

                entity.HasOne(d => d.DSubjectNavigation)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.DSubject)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Courses_ibfk_1");
            });

            modelBuilder.Entity<Departments>(entity =>
            {
                entity.HasKey(e => e.DSubject)
                    .HasName("PRIMARY");

                entity.Property(e => e.DSubject)
                    .HasColumnName("dSubject")
                    .HasColumnType("varchar(4)");

                entity.Property(e => e.DName)
                    .IsRequired()
                    .HasColumnName("dName")
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<Enrollments>(entity =>
            {
                entity.HasKey(e => e.EId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.UserId)
                    .HasName("userID");

                entity.HasIndex(e => new { e.ClassId, e.UserId })
                    .HasName("classID")
                    .IsUnique();

                entity.Property(e => e.EId).HasColumnName("eID");

                entity.Property(e => e.ClassId).HasColumnName("classID");

                entity.Property(e => e.Grade)
                    .IsRequired()
                    .HasColumnName("grade")
                    .HasColumnType("varchar(2)");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("userID")
                    .HasColumnType("char(8)");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Enrollments_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("Enrollments_ibfk_1");
            });

            modelBuilder.Entity<Professors>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Dept)
                    .HasName("dept");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("char(8)");

                entity.Property(e => e.Dept)
                    .IsRequired()
                    .HasColumnName("dept")
                    .HasColumnType("varchar(4)");

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.FName)
                    .IsRequired()
                    .HasColumnName("fName")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.LName)
                    .IsRequired()
                    .HasColumnName("lName")
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.DeptNavigation)
                    .WithMany(p => p.Professors)
                    .HasForeignKey(d => d.Dept)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Professors_ibfk_1");
            });

            modelBuilder.Entity<Students>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.DSubject)
                    .HasName("dSubject");

                entity.Property(e => e.UserId)
                    .HasColumnName("userID")
                    .HasColumnType("char(8)");

                entity.Property(e => e.DSubject)
                    .IsRequired()
                    .HasColumnName("dSubject")
                    .HasColumnType("varchar(4)");

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.FName)
                    .IsRequired()
                    .HasColumnName("fName")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.LName)
                    .IsRequired()
                    .HasColumnName("lName")
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.DSubjectNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.DSubject)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Students_ibfk_1");
            });

            modelBuilder.Entity<Submissions>(entity =>
            {
                entity.HasKey(e => e.SubId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.AId)
                    .HasName("aID");

                entity.HasIndex(e => new { e.UserId, e.AId })
                    .HasName("userID")
                    .IsUnique();

                entity.Property(e => e.SubId).HasColumnName("subID");

                entity.Property(e => e.AId).HasColumnName("aID");

                entity.Property(e => e.Score).HasColumnName("score");

                entity.Property(e => e.SubContents)
                    .IsRequired()
                    .HasColumnName("subContents")
                    .HasColumnType("varchar(8192)");

                entity.Property(e => e.SubTime)
                    .HasColumnName("subTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("userID")
                    .HasColumnType("char(8)");

                entity.HasOne(d => d.A)
                    .WithMany(p => p.Submissions)
                    .HasForeignKey(d => d.AId)
                    .HasConstraintName("Submissions_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Submissions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("Submissions_ibfk_1");
            });
        }
    }
}
