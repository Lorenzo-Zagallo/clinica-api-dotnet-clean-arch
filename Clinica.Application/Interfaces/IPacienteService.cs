using Clinica.Application.DTOs;
using Clinica.Domain.Entities;

namespace Clinica.Application.Interfaces;

public interface IPacienteService
{
    Task<Paciente> CadastrarPacienteAsync(CriarPacienteDTO dto);
    Task<List<Paciente>> ListarPacientesAsync();
    Task<bool> AtualizarPacienteAsync(int pacienteId, CriarPacienteDTO dto);
    Task<bool> DeletarPacienteAsync(int pacienteId);

    Task<DashboardDTO> ObterDadosDashboardAsync();
}