using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using ClinicManagerAPI.Models.Entities;

namespace ClinicManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialtyController : ControllerBase
    {
        private readonly IConfiguration _config;
        public SpecialtyController(IConfiguration config) => _config = config;
        private SqlConnection GetConnection() => new(_config.GetConnectionString("ClinicManagerDB"));

        /// <summary>
        /// Obtiene todas las especialidades activas
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var specialties = new List<Specialty>();
                using var connection = GetConnection();
                using var command = new SqlCommand("sp_specialty_get_all", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    specialties.Add(MapSpecialtyFromReader(reader));
                }

                return Ok(specialties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error interno: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtiene una especialidad por su código
        /// </summary>
        /// <param name="code">Código de la especialidad</param>
        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            try
            {
                using var connection = GetConnection();
                using var command = new SqlCommand("sp_specialty_get_by_code", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@code", code);

                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();

                if (!reader.HasRows)
                    return NotFound(new { message = "Especialidad no encontrada" });

                await reader.ReadAsync();
                return Ok(MapSpecialtyFromReader(reader));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error interno: {ex.Message}" });
            }
        }

        /// <summary>
        /// Crea una nueva especialidad
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Specialty specialty)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                using var connection = GetConnection();
                using var command = new SqlCommand("sp_specialty_create", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@code", specialty.Code);
                command.Parameters.AddWithValue("@name", specialty.Name);
                command.Parameters.AddWithValue("@description",
                    string.IsNullOrEmpty(specialty.Description) ? DBNull.Value : specialty.Description);

                await connection.OpenAsync();
                var result = await command.ExecuteScalarAsync();

                return Ok(new
                {
                    success = true,
                    message = result?.ToString() ?? "Especialidad creada exitosamente",
                    code = specialty.Code
                });
            }
            catch (SqlException ex) when (ex.Number == 2627) // Violación de unique key
            {
                return Conflict(new { error = "El código de especialidad ya existe" });
            }
            catch (SqlException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error interno: {ex.Message}" });
            }
        }

        /// <summary>
        /// Actualiza una especialidad existente
        /// </summary>
        /// <param name="code">Código de la especialidad a actualizar</param>
        [HttpPut("{code}")]
        public async Task<IActionResult> Update(string code, [FromBody] Specialty specialty)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                using var connection = GetConnection();
                using var command = new SqlCommand("sp_specialty_update", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@code", code);
                command.Parameters.AddWithValue("@name", specialty.Name);
                command.Parameters.AddWithValue("@description",
                    string.IsNullOrEmpty(specialty.Description) ? DBNull.Value : specialty.Description);

                await connection.OpenAsync();
                var affectedRows = await command.ExecuteNonQueryAsync();

                if (affectedRows == 0)
                    return NotFound(new { message = "Especialidad no encontrada" });

                return Ok(new
                {
                    success = true,
                    message = "Especialidad actualizada exitosamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error interno: {ex.Message}" });
            }
        }

        /// <summary>
        /// Desactiva una especialidad (eliminación lógica)
        /// </summary>
        /// <param name="code">Código de la especialidad a desactivar</param>
        [HttpDelete("{code}")]
        public async Task<IActionResult> Deactivate(string code)
        {
            try
            {
                using var connection = GetConnection();
                using var command = new SqlCommand("sp_specialty_deactivate", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@code", code);

                await connection.OpenAsync();
                var affectedRows = await command.ExecuteNonQueryAsync();

                if (affectedRows == 0)
                    return NotFound(new { message = "Especialidad no encontrada" });

                return Ok(new
                {
                    success = true,
                    message = "Especialidad desactivada exitosamente"
                });
            }
            catch (SqlException ex) when (ex.Number == 547) // FK constraint
            {
                return BadRequest(new { error = "No se puede desactivar, existen médicos asociados" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error interno: {ex.Message}" });
            }
        }

        // Método auxiliar para mapear desde SqlDataReader
        private Specialty MapSpecialtyFromReader(SqlDataReader reader)
        {
            return new Specialty
            {
                Id = reader.GetInt32(reader.GetOrdinal("spc_id")),
                Code = reader.GetString(reader.GetOrdinal("spc_code")),
                Name = reader.GetString(reader.GetOrdinal("spc_name")),
                Description = reader.IsDBNull(reader.GetOrdinal("spc_description")) ?
                    null : reader.GetString(reader.GetOrdinal("spc_description"))
            };
        }
    }
}