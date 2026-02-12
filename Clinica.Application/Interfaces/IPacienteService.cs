using Clinica.Application.DTOs;
using Clinica.Domain.Entities;

namespace Clinica.Application.Interfaces;

public interface IPacienteService
{
    Task<Paciente> CadastrarPacienteAsync(CreatePacienteDTO dto);
    Task<List<Paciente>> ListarPacientesAsync();
    Task<bool> AtualizarPacienteAsync(int pacienteId, CreatePacienteDTO dto);
    Task<bool> DeletarPacienteAsync(int pacienteId);

    Task<DashboardDTO> ObterDadosDashboardAsync();
}