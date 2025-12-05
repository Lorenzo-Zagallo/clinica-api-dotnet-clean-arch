using Clinica.Application.DTOs;
using Clinica.Application.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinica.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PacientesController : ControllerBase
{
    private readonly IPacienteService _pacienteService;

    // O controller pede o Service, que pede o Repository, que pede o Banco.
    // Tudo conectado automaticamente no Program.cs pelo .NET via Injeção de Dependência do ASP.NET Core DI Container
    public PacientesController(IPacienteService pacienteService)
    {
        _pacienteService = pacienteService;
    }

    // POST: api/pacientes
    [HttpPost]
    public async Task<IActionResult> CadastrarPaciente([FromBody] CriarPacienteDTO dto)
    {
        // Se o JSON vier vazio ou errado, o [ApiController] já valida automaticamente
        var pacienteCriado = await _pacienteService.CadastrarPacienteAsync(dto);

        // Retorna status 201 (Created) com: o paciente criado no corpo da resposta e o local para acessar esse paciente via GET
        return CreatedAtAction(nameof(CadastrarPaciente), new { id = pacienteCriado.Id }, pacienteCriado);
    }

    // GET: api/pacientes
    [HttpGet]
    public async Task<IActionResult> ListarPacientes()
    {
        var pacientes = await _pacienteService.ListarPacientesAsync();
        return Ok(pacientes);
    }

    // PUT: api/pacientes/{pacienteId}
    [HttpPut("{pacienteId}")]
    public async Task<IActionResult> AtualizarPaciente(int pacienteId, [FromBody] CriarPacienteDTO dto)
    {
        var sucesso = await _pacienteService.AtualizarPacienteAsync(pacienteId, dto);

        if (!sucesso) return NotFound(); // Retorna 404 se o paciente não existir

        return NoContent(); // Retorna 204 se a atualização foi bem-sucedida
    }

    // DELETE: api/pacientes/{pacienteId}
    [HttpDelete("{pacienteId}")]
    public async Task<IActionResult> DeletarPaciente(int pacienteId)
    {
        var sucesso = await _pacienteService.DeletarPacienteAsync(pacienteId);

        if (!sucesso) return NotFound(); // Retorna 404 se o paciente não existir

        return NoContent(); // Retorna 204 se a deleção foi bem-sucedida
    }
}