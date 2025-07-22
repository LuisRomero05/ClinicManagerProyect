using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using ClinicManagerAPI.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IConfiguration _config;
        public PatientController(IConfiguration config) => _config = config;
        private SqlConnection GetConn() => new(_config.GetConnectionString("ClinicManagerDB"));

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? searchTerm = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var list = new List<Patient>();
            using var conn = GetConn();
            using var cmd = new SqlCommand("sp_patient_search", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@search_term", (object?)searchTerm ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@page", page);
            cmd.Parameters.AddWithValue("@page_size", pageSize);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Patient
                {
                    Id = reader.GetInt32(0),
                    Identity = reader.GetString(1),
                    Code = reader.GetString(2),
                    Name = reader.GetString(3),
                    Email = reader.GetString(4),
                    Phone = reader.IsDBNull(5) ? null : reader.GetString(5),
                    Address = reader.IsDBNull(6) ? null : reader.GetString(6),
                    Birthdate = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                    IsActive = reader.GetBoolean(8)
                });
            }
            return Ok(list);
        }

        [HttpGet("{identifier}")]
        public async Task<IActionResult> GetByIdentifier(string identifier)
        {
            using var conn = GetConn();
            using var cmd = new SqlCommand("sp_patient_get_by_id", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@identifier", identifier);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (!reader.HasRows) return NotFound();

            await reader.ReadAsync();
            return Ok(new Patient
            {
                Id = reader.GetInt32(0),
                Identity = reader.GetString(1),
                Code = reader.GetString(2),
                Name = reader.GetString(3),
                Email = reader.GetString(4),
                Phone = reader.IsDBNull(5) ? null : reader.GetString(5),
                Address = reader.IsDBNull(6) ? null : reader.GetString(6),
                Birthdate = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                IsActive = reader.GetBoolean(8)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PatientCreateDto patientDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using var conn = GetConn();
            using var cmd = new SqlCommand("sp_patient_create", conn) { CommandType = CommandType.StoredProcedure };

            // Generar código de paciente
            var LastPatient = await GetLastPatientCode();
            var patientCode = LastPatient == null ? "PA-001" : $"PA-{(int.Parse(LastPatient.Split('-')[1]) + 1):D3}";

            cmd.Parameters.AddWithValue("@identity", patientDto.Identity);
            cmd.Parameters.AddWithValue("@code", patientCode);
            cmd.Parameters.AddWithValue("@name", patientDto.Name);
            cmd.Parameters.AddWithValue("@email", patientDto.Email);
            cmd.Parameters.AddWithValue("@password", HashPassword(patientDto.Password));
            cmd.Parameters.AddWithValue("@phone", (object?)patientDto.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@address", (object?)patientDto.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@birthdate", (object?)patientDto.Birthdate ?? DBNull.Value);

            try
            {
                await conn.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();
                return Ok(new
                {
                    message = result?.ToString() ?? "Paciente creado exitosamente.",
                    code = patientCode
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{code}")]
        public async Task<IActionResult> Update(string code, [FromBody] PatientUpdateDto patientDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using var conn = GetConn();
            using var cmd = new SqlCommand("sp_patient_update", conn) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@code", code);
            cmd.Parameters.AddWithValue("@name", patientDto.Name);
            cmd.Parameters.AddWithValue("@email", patientDto.Email);
            cmd.Parameters.AddWithValue("@phone", (object?)patientDto.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@address", (object?)patientDto.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@birthdate", (object?)patientDto.Birthdate ?? DBNull.Value);

            try
            {
                await conn.OpenAsync();
                var affectedRows = await cmd.ExecuteNonQueryAsync();
                if (affectedRows == 0) return NotFound();
                return Ok(new { message = "Paciente actualizado exitosamente." });
            }
            catch (SqlException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> Deactivate(string code)
        {
            using var conn = GetConn();
            using var cmd = new SqlCommand("sp_patient_deactivate", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@code", code);

            try
            {
                await conn.OpenAsync();
                var affectedRows = await cmd.ExecuteNonQueryAsync();
                if (affectedRows == 0) return NotFound();
                return Ok(new { message = "Paciente desactivado exitosamente." });
            }
            catch (SqlException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        private async Task<string?> GetLastPatientCode()
        {
            using var conn = GetConn();
            using var cmd = new SqlCommand("SELECT TOP 1 pat_code FROM patients ORDER BY pat_id DESC", conn);
            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return result?.ToString();
        }

        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public class PatientCreateDto
        {
            [Required(ErrorMessage = "La identidad es requerida")]
            [StringLength(20, ErrorMessage = "La identidad no puede exceder 20 caracteres")]
            public string Identity { get; set; }

            [Required(ErrorMessage = "El nombre es requerido")]
            [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
            public string Name { get; set; }

            [Required(ErrorMessage = "El email es requerido")]
            [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
            public string Email { get; set; }

            [Required(ErrorMessage = "La contraseña es requerida")]
            [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
            public string Password { get; set; }

            [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
            public string? Phone { get; set; }

            public string? Address { get; set; }

            public DateTime? Birthdate { get; set; }
        }

        public class PatientUpdateDto
        {
            [Required(ErrorMessage = "El nombre es requerido")]
            [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
            public string Name { get; set; }

            [Required(ErrorMessage = "El email es requerido")]
            [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
            public string Email { get; set; }

            [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
            public string? Phone { get; set; }

            public string? Address { get; set; }

            public DateTime? Birthdate { get; set; }
        }
    }
}