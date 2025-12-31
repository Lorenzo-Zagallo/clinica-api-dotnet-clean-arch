using Clinica.Application.DTOs;
using Clinica.Application.Interfaces;
using Clinica.Domain.Entities;
using Clinica.Domain.Interfaces;

namespace Clinica.Application.Services;

public class PacienteService : IPacienteService
{
    private readonly IPacienteRepository _pacienteRepository;

    public PacienteService(IPacienteRepository pacienteRepository)
    {
        _pacienteRepository = pacienteRepository;
    }

    public async Task<Paciente> CadastrarPacienteAsync(CriarPacienteDTO dto)
    {
        if (dto.ValorConsulta < 0)
        {
            throw new Exception("Valor da consulta não pode ser negativo");
        }
        
        // Aqui transformamos o DTO (dados da tela) em Entidade (dados do banco)
        var paciente = new Paciente
        {
            Nome = dto.Nome,
            CPF = dto.CPF,
            ValorConsulta = dto.ValorConsulta,
            DataCadastro = DateTime.Now // Regra de negócio: data é automática
        };

        // Chamamos o repositório para salvar no banco de dados usando a entidade criada acima
        return await _pacienteRepository.CriarAsync(paciente);
    }
    
    public async Task<List<Paciente>> ListarPacientesAsync()
    {
        // Chamamos o repositório para obter todos os pacientes do banco de dados
        return await _pacienteRepository.ObterTodosAsync();
    }

    public async Task<bool> AtualizarPacienteAsync(int pacienteId, CriarPacienteDTO dto)
    {
        var pacienteExistente = await _pacienteRepository.ObterPorIdAsync(pacienteId);
        
        if (pacienteExistente == null) return false;

        // Atualiza os dados
        pacienteExistente.Nome = dto.Nome;
        pacienteExistente.CPF = dto.CPF;
        pacienteExistente.ValorConsulta = dto.ValorConsulta;

        await _pacienteRepository.AtualizarAsync(pacienteExistente);
        return true;
    }

    public async Task<bool> DeletarPacienteAsync(int pacienteId)
    {
        var pacienteExistente = await _pacienteRepository.ObterPorIdAsync(pacienteId);

        if (pacienteExistente == null) return false;

        await _pacienteRepository.DeletarAsync(pacienteExistente);
        return true;
    }

    public async Task<DashboardDTO> ObterDadosDashboardAsync()
    {
        var total = await _pacienteRepository.ContarTotalAsync();
        var faturamento = await _pacienteRepository.SomarValorTotalAsync();

        // Evitar divisão por zero
        var media = total > 0 ? faturamento / total : 0;

        return new DashboardDTO
        {
            TotalPacientes = total,
            FaturamentoTotal = faturamento,
            MediaValorConsulta = Math.Round(media, 2)
        };
    }
}