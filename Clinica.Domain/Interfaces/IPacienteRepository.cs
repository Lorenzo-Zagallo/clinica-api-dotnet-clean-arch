using Clinica.Domain.Entities;

namespace Clinica.Domain.Interfaces;

public interface IPacienteRepository
{
    // Definimos O QUE o sistema faz, mas não COMO faz
    Task<Paciente> CriarAsync(Paciente paciente);
    Task<List<Paciente>> ObterTodosAsync();
    Task<Paciente?> ObterPorIdAsync(int pacienteId);
    Task AtualizarAsync(Paciente paciente);
    Task DeletarAsync(Paciente paciente);
}