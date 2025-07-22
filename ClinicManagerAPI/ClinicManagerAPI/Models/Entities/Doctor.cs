namespace ClinicManagerAPI.Models.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public string License { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // Faltaba esta propiedad
        public string? Phone { get; set; }
        public int SpecialtyId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Propiedades de navegación
        public Specialty? Specialty { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
    }
}