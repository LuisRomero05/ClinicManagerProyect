using System.ComponentModel.DataAnnotations;

namespace ClinicManagerAPI.Models.DTOS.Doctor
{
    public class DoctorCreateDto
    {
        [Required, StringLength(20)]
        public string License { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(8)]
        public string Password { get; set; }

        [Phone, StringLength(20)]
        public string Phone { get; set; }

        [Required]
        public int SpecialtyId { get; set; }
    }
}
