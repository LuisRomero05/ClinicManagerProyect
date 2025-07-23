namespace ClinicManagerAPI2.Models.DTOS.Appointment
{
    public class AppointmentResponseDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string SpecialtyName { get; set; }
        public string Notes { get; set; }
    }
}
