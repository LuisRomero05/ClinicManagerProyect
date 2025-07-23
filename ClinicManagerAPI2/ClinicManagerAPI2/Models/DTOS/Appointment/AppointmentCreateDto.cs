namespace ClinicManagerAPI2.Models.DTOS.Appointment
{
    public class AppointmentCreateDto
    {
        public string PatientIdentifier { get; set; }
        public string DoctorIdentifier { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public int DurationMinutes { get; set; }
        public string Notes { get; set; }
    }
}