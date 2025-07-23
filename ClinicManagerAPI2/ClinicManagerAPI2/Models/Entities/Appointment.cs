using System.Numerics;

namespace ClinicManagerAPI2.Models.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int SpecialtyId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; } = "scheduled";
        public string Notes { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Propiedades de navegación
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
        public Specialty Specialty { get; set; }
    }
}