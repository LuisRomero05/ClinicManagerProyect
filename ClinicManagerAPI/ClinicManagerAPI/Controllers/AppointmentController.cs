using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using ClinicManagerAPI.Models.Entities;
using ClinicManagerAPI.Classes;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public AppointmentController(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        private SqlConnection GetConn() => new(_config.GetConnectionString("ClinicManagerDB"));

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string searchTerm = null,
            [FromQuery] string dateFrom = null,
            [FromQuery] string dateTo = null,
            [FromQuery] string status = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                // Usando Entity Framework para evitar problemas de mapeo
                var query = _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .Include(a => a.Specialty)
                    .Where(a => a.IsActive);

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(a =>
                        a.Code.Contains(searchTerm) ||
                        a.Patient.Name.Contains(searchTerm) ||
                        a.Doctor.Name.Contains(searchTerm));
                }

                if (!string.IsNullOrEmpty(dateFrom) && DateTime.TryParse(dateFrom, out var fromDate))
                {
                    query = query.Where(a => a.Date >= fromDate);
                }

                if (!string.IsNullOrEmpty(dateTo) && DateTime.TryParse(dateTo, out var toDate))
                {
                    query = query.Where(a => a.Date <= toDate);
                }

                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(a => a.Status == status);
                }

                var totalRecords = await query.CountAsync();
                var appointments = await query
                    .OrderBy(a => a.Date)
                    .ThenBy(a => a.StartTime)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(a => new
                    {
                        a.Id,
                        a.Code,
                        a.Date,
                        a.StartTime,
                        a.EndTime,
                        a.Status,
                        Patient = new { a.Patient.Code, a.Patient.Name },
                        Doctor = new { a.Doctor.Code, a.Doctor.Name },
                        Specialty = new { a.Specialty.Name }
                    })
                    .ToListAsync();

                return Ok(new
                {
                    data = appointments,
                    totalRecords,
                    page,
                    pageSize,
                    totalPages = (int)Math.Ceiling((double)totalRecords / pageSize)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .Include(a => a.Specialty)
                    .Where(a => a.Code == code && a.IsActive)
                    .Select(a => new
                    {
                        a.Id,
                        a.Code,
                        a.Date,
                        a.StartTime,
                        a.EndTime,
                        a.Status,
                        a.Notes,
                        Patient = new { a.Patient.Id, a.Patient.Code, a.Patient.Identity, a.Patient.Name },
                        Doctor = new { a.Doctor.Id, a.Doctor.Code, a.Doctor.License, a.Doctor.Name },
                        Specialty = new { a.Specialty.Id, a.Specialty.Code, a.Specialty.Name },
                        a.CreatedAt,
                        a.UpdatedAt
                    })
                    .FirstOrDefaultAsync();

                if (appointment == null)
                    return NotFound(new { message = "Cita no encontrada." });

                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p =>
                        (p.Code == request.PatientIdentifier || p.Identity == request.PatientIdentifier) &&
                        p.IsActive);

                if (patient == null)
                    return BadRequest(new { error = "Paciente no encontrado" });

                var doctor = await _context.Doctors
                    .FirstOrDefaultAsync(d =>
                        (d.Code == request.DoctorIdentifier || d.License == request.DoctorIdentifier) &&
                        d.IsActive);

                if (doctor == null)
                    return BadRequest(new { error = "Médico no encontrado" });

                var specialty = await _context.Specialties
                    .FirstOrDefaultAsync(s => s.Id == doctor.SpecialtyId && s.IsActive);

                if (specialty == null)
                    return BadRequest(new { error = "Especialidad no encontrada" });

                // Generar código de cita
                var nextSeq = await _context.Appointments
                    .Where(a => a.Code.StartsWith("APT-") && a.CreatedAt.Year == DateTime.Now.Year)
                    .CountAsync() + 1;

                var appointmentCode = $"APT-{DateTime.Now:yyyy}-{nextSeq:D3}";

                var appointment = new Appointment
                {
                    Code = appointmentCode,
                    PatientId = patient.Id,
                    DoctorId = doctor.Id,
                    SpecialtyId = specialty.Id,
                    Date = request.Date,
                    StartTime = request.StartTime,
                    EndTime = request.StartTime.Add(TimeSpan.FromMinutes(request.DurationMinutes ?? 30)),
                    Status = "scheduled",
                    Notes = request.Notes,
                    IsActive = true
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Cita creada exitosamente.",
                    appointmentCode,
                    success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("{code}/status")]
        public async Task<IActionResult> UpdateStatus(string code, [FromBody] UpdateStatusRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var appointment = await _context.Appointments
                    .FirstOrDefaultAsync(a => a.Code == code && a.IsActive);

                if (appointment == null)
                    return NotFound(new { message = "Cita no encontrada." });

                appointment.Status = request.Status;
                appointment.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Estado actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("{code}")]
        public async Task<IActionResult> Update(string code, [FromBody] UpdateAppointmentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var appointment = await _context.Appointments
                    .FirstOrDefaultAsync(a => a.Code == code && a.IsActive);

                if (appointment == null)
                    return NotFound(new { message = "Cita no encontrada." });

                appointment.Notes = request.Notes;
                appointment.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Cita actualizada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete("{code}/cancel")]
        public async Task<IActionResult> Cancel(string code, [FromBody] CancelAppointmentRequest request = null)
        {
            try
            {
                var appointment = await _context.Appointments
                    .FirstOrDefaultAsync(a => a.Code == code && a.IsActive);

                if (appointment == null)
                    return NotFound(new { message = "Cita no encontrada." });

                // Validar que no se cancele con menos de 24 horas de anticipación
                var appointmentDateTime = appointment.Date.Add(appointment.StartTime);
                if ((appointmentDateTime - DateTime.Now).TotalHours < 24)
                    return BadRequest(new { error = "No se puede cancelar con menos de 24 horas de anticipación." });

                appointment.Status = "canceled";
                appointment.IsActive = false;
                appointment.Notes = request?.Notes ?? appointment.Notes;
                appointment.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Cita cancelada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> Deactivate(string code)
        {
            try
            {
                var appointment = await _context.Appointments
                    .FirstOrDefaultAsync(a => a.Code == code && a.IsActive);

                if (appointment == null)
                    return NotFound(new { message = "Cita no encontrada." });

                appointment.IsActive = false;
                appointment.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Cita desactivada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

    public class CreateAppointmentRequest
    {
        public string PatientIdentifier { get; set; }
        public string DoctorIdentifier { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public int? DurationMinutes { get; set; } = 30;
        public string Notes { get; set; }
    }

    public class UpdateStatusRequest
    {
        public string Status { get; set; }
    }

    public class UpdateAppointmentRequest
    {
        public string Notes { get; set; }
    }

    public class CancelAppointmentRequest
    {
        public string Notes { get; set; }
    }
}