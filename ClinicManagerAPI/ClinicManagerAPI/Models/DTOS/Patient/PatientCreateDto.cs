namespace ClinicManagerAPI.Models.DTOS.Patient
{
    public class PatientCreateDto
    {
        public string Identity { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}
