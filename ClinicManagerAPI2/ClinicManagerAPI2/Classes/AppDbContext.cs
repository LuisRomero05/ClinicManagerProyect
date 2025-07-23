using ClinicManagerAPI2.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace ClinicManagerAPI2.Classes
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuraciones del modelo para Patient
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("patients");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasColumnName("pat_id");
                entity.Property(p => p.Identity).HasColumnName("pat_identity").IsRequired();
                entity.Property(p => p.Code).HasColumnName("pat_code").IsRequired();
                entity.Property(p => p.Name).HasColumnName("pat_name").IsRequired();
                entity.Property(p => p.Email).HasColumnName("pat_email").IsRequired();
                entity.Property(p => p.Password).HasColumnName("pat_password").IsRequired();
                entity.Property(p => p.Phone).HasColumnName("pat_phone");
                entity.Property(p => p.Address).HasColumnName("pat_address");
                entity.Property(p => p.Birthdate).HasColumnName("pat_birthdate");
                entity.Property(p => p.IsActive).HasColumnName("pat_is_active").HasDefaultValue(true);
                entity.Property(p => p.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
                entity.Property(p => p.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("GETDATE()");
            });

            // Configuración de Doctor
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("doctors");
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Id).HasColumnName("doc_id");
                entity.Property(d => d.License).HasColumnName("doc_license").IsRequired();
                entity.Property(d => d.Code).HasColumnName("doc_code").IsRequired();
                entity.Property(d => d.Name).HasColumnName("doc_name").IsRequired();
                entity.Property(d => d.Email).HasColumnName("doc_email").IsRequired();
                entity.Property(d => d.Password).HasColumnName("doc_password").IsRequired();
                entity.Property(d => d.Phone).HasColumnName("doc_phone");
                entity.Property(d => d.SpecialtyId).HasColumnName("doc_specialty_id").IsRequired();
                entity.Property(d => d.IsActive).HasColumnName("doc_is_active").HasDefaultValue(true);
                entity.Property(d => d.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
                entity.Property(d => d.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("GETDATE()");
            });

            // Configuración de Specialty
            modelBuilder.Entity<Specialty>(entity =>
            {
                entity.ToTable("specialties");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id).HasColumnName("spc_id");
                entity.Property(s => s.Code).HasColumnName("spc_code").IsRequired();
                entity.Property(s => s.Name).HasColumnName("spc_name").IsRequired();
                entity.Property(s => s.Description).HasColumnName("spc_description");
                entity.Property(s => s.IsActive).HasColumnName("spc_is_active").HasDefaultValue(true);
                entity.Property(s => s.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
                entity.Property(s => s.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("GETDATE()");
            });

            // Configuración de Appointment
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("appointments");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Id).HasColumnName("apt_id");
                entity.Property(a => a.Code).HasColumnName("apt_code").IsRequired();
                entity.Property(a => a.PatientId).HasColumnName("apt_patient_id").IsRequired();
                entity.Property(a => a.DoctorId).HasColumnName("apt_doctor_id").IsRequired();
                entity.Property(a => a.SpecialtyId).HasColumnName("apt_specialty_id").IsRequired();
                entity.Property(a => a.Date).HasColumnName("apt_date").IsRequired();
                entity.Property(a => a.StartTime).HasColumnName("apt_start_time").IsRequired();
                entity.Property(a => a.EndTime).HasColumnName("apt_end_time").IsRequired();
                entity.Property(a => a.Status).HasColumnName("apt_status").HasDefaultValue("scheduled");
                entity.Property(a => a.Notes).HasColumnName("apt_notes");
                entity.Property(a => a.IsActive).HasColumnName("apt_is_active").HasDefaultValue(true);
                entity.Property(a => a.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
                entity.Property(a => a.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("GETDATE()");
            });

            // Relaciones
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Specialty)
                .WithMany(s => s.Appointments)
                .HasForeignKey(a => a.SpecialtyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            modelBuilder.Entity<Specialty>()
                .HasIndex(s => s.Code)
                .IsUnique();

            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.Identity)
                .IsUnique();

            modelBuilder.Entity<Patient>()
            .HasIndex(p => p.Email)
                .IsUnique();

            modelBuilder.Entity<Doctor>()
            .HasIndex(d => d.License)
                .IsUnique();

            modelBuilder.Entity<Doctor>()
                .HasIndex(d => d.Email)
                .IsUnique();

            modelBuilder.Entity<Appointment>()
                .HasIndex(a => a.Code)
                .IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-F3NGIPI\\MSSQLSERVER02;Database=MedicalAppointmentManager;Trusted_Connection=True;TrustServerCertificate=True;")
                    .EnableSensitiveDataLogging()
                    .LogTo(Console.WriteLine, LogLevel.Information);
            }
        }
    }
}