using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ClinicManagerAPI2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly IConfiguration _config;
        public DoctorController(IConfiguration config) => _config = config;
        private SqlConnection GetConn() => new(_config.GetConnectionString("ClinicManagerDB"));

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = new List<object>();
            using var conn = GetConn();
            using var cmd = new SqlCommand("sp_doctor_search", conn) { CommandType = CommandType.StoredProcedure };
            // Parámetros por defecto para obtener todos
            cmd.Parameters.AddWithValue("@search_term", DBNull.Value);
            cmd.Parameters.AddWithValue("@specialty_id", DBNull.Value);
            cmd.Parameters.AddWithValue("@page", 1);
            cmd.Parameters.AddWithValue("@page_size", 1000); // Valor alto para obtener todos

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new
                {
                    Id = reader.GetInt32("doc_id"),
                    License = reader.GetString("doc_license"),
                    Code = reader.GetString("doc_code"),
                    Name = reader.GetString("doc_name"),
                    Email = reader.GetString("doc_email"),
                    Phone = reader.IsDBNull("doc_phone") ? null : reader.GetString("doc_phone"),
                    SpecialtyName = reader.GetString("specialty_name")
                });
            }
            return Ok(list);
        }

        [HttpGet("{identifier}")]
        public async Task<IActionResult> GetByIdentifier(string identifier)
        {
            using var conn = GetConn();
            using var cmd = new SqlCommand("sp_doctor_get_by_id", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@identifier", identifier);

            try
            {
                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                if (!reader.HasRows) return NotFound();

                await reader.ReadAsync();
                return Ok(new
                {
                    Id = reader.GetInt32("doc_id"),
                    License = reader.GetString("doc_license"),
                    Code = reader.GetString("doc_code"),
                    Name = reader.GetString("doc_name"),
                    Email = reader.GetString("doc_email"),
                    Phone = reader.IsDBNull("doc_phone") ? null : reader.GetString("doc_phone"),
                    SpecialtyId = reader.GetInt32("doc_specialty_id"),
                    SpecialtyName = reader.GetString("specialty_name"),
                    CreatedAt = reader.GetDateTime("created_at"),
                    UpdatedAt = reader.GetDateTime("updated_at")
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? searchTerm = null,
            [FromQuery] int? specialtyId = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var doctors = new List<object>();
            int totalRecords = 0;

            using var conn = GetConn();
            using var cmd = new SqlCommand("sp_doctor_search", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@search_term", (object?)searchTerm ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@specialty_id", (object?)specialtyId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@page", page);
            cmd.Parameters.AddWithValue("@page_size", pageSize);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            // Primer resultado: datos de médicos
            while (await reader.ReadAsync())
            {
                doctors.Add(new
                {
                    Id = reader.GetInt32("doc_id"),
                    License = reader.GetString("doc_license"),
                    Code = reader.GetString("doc_code"),
                    Name = reader.GetString("doc_name"),
                    Email = reader.GetString("doc_email"),
                    Phone = reader.IsDBNull("doc_phone") ? null : reader.GetString("doc_phone"),
                    SpecialtyName = reader.GetString("specialty_name")
                });
            }

            // Segundo resultado: total de registros
            if (await reader.NextResultAsync() && await reader.ReadAsync())
            {
                totalRecords = reader.GetInt32("total_records");
            }

            return Ok(new
            {
                data = doctors,
                totalRecords,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSize)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DoctorCreateRequest doctor)
        {
            // Generar código automáticamente
            string newCode = await GenerateNextDoctorCode();

            using var conn = GetConn();
            using var cmd = new SqlCommand("sp_doctor_create", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@license", doctor.License);
            cmd.Parameters.AddWithValue("@code", newCode);
            cmd.Parameters.AddWithValue("@name", doctor.Name);
            cmd.Parameters.AddWithValue("@email", doctor.Email);
            cmd.Parameters.AddWithValue("@password", doctor.Password);
            cmd.Parameters.AddWithValue("@phone", (object?)doctor.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@specialty_id", doctor.SpecialtyId);

            try
            {
                await conn.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();
                return Ok(new
                {
                    message = result?.ToString() ?? "Médico creado exitosamente.",
                    code = newCode
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{code}")]
        public async Task<IActionResult> Update(string code, [FromBody] DoctorUpdateRequest doctor)
        {
            using var conn = GetConn();
            using var cmd = new SqlCommand("sp_doctor_update", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@code", code);
            cmd.Parameters.AddWithValue("@name", doctor.Name);
            cmd.Parameters.AddWithValue("@email", doctor.Email);
            cmd.Parameters.AddWithValue("@phone", (object?)doctor.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@specialty_id", doctor.SpecialtyId);

            try
            {
                await conn.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();
                return Ok(new { message = result?.ToString() ?? "Médico actualizado exitosamente." });
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
            using var cmd = new SqlCommand("sp_doctor_deactivate", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@code", code);

            try
            {
                await conn.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();
                return Ok(new { message = result?.ToString() ?? "Médico desactivado exitosamente." });
            }
            catch (SqlException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        private async Task<string> GenerateNextDoctorCode()
        {
            using var conn = GetConn();
            using var cmd = new SqlCommand(
                "SELECT TOP 1 doc_code FROM doctors WHERE doc_code LIKE 'DOC-%' ORDER BY doc_code DESC",
                conn);

            await conn.OpenAsync();
            var lastCode = await cmd.ExecuteScalarAsync() as string;

            if (string.IsNullOrEmpty(lastCode))
                return "DOC-001";

            var number = int.Parse(lastCode.Split('-')[1]);
            return $"DOC-{(number + 1):D3}";
        }
    }

    // DTOs para las peticiones
    public class DoctorCreateRequest
    {
        public string License { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public int SpecialtyId { get; set; }
    }

    public class DoctorUpdateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public int SpecialtyId { get; set; }
    }
}