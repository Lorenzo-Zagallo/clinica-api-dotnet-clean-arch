using Clinica.Application.DTOs;
using Clinica.Domain.Entities;
using Clinica.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clinica.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultasController : ControllerBase
    {
        private readonly ClinicaDbContext _context;

        public ConsultasController(ClinicaDbContext context)
        {
            _context = context;
        }

        // POST: api/consultas (Agendar nova consulta)
        [HttpPost]
        public async Task<IActionResult> Agendar([FromBody] CreateConsultaDTO dto)
        {
            // 1. Validação: O paciente existe?
            var paciente = await _context.Pacientes.FindAsync(dto.PacienteId);
            if (paciente == null)
                return NotFound("Paciente não encontrado.");

            // 2. Mapeamento (DTO -> Entidade)
            var consulta = new Consulta
            {
                PacienteId = dto.PacienteId,
                DataHorario = dto.DataHorario,
                Tipo = dto.Tipo,
                Observacoes = dto.Observacoes,
                Valor = dto.Valor,
                Status = "Agendado" // Status inicial padrão
            };

            // 3. Salvar no Banco
            _context.Consultas.Add(consulta);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ListarPorId), new { id = consulta.Id }, consulta);
        }

        // GET: api/consultas (Listar todas as consultas com dados do paciente)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadConsultaDTO>>> ListarTodas()
        {
            var consultas = await _context.Consultas
                .Include(c => c.Paciente) // JOIN com a tabela Pacientes
                .Select(c => new ReadConsultaDTO
                {
                    Id = c.Id,
                    DataHorario = c.DataHorario,
                    Tipo = c.Tipo,
                    Status = c.Status,
                    Valor = c.Valor,
                    PacienteId = c.PacienteId,
                    NomePaciente = c.Paciente != null ? c.Paciente.Nome : "Desconhecido"
                })
                .ToListAsync();
            
            return Ok(consultas);
        }

        // GET: api/consultas/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> ListarPorId(int id)
        {
            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta == null) return NotFound();
            return Ok(consulta);
        }
    }
}