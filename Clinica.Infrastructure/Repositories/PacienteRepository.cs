using Clinica.Domain.Entities;
using Clinica.Domain.Interfaces;
using Clinica.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Clinica.Infrastructure.Repositories;

public class PacienteRepository : IPacienteRepository
{
    private readonly ClinicaDbContext _context;

    // Injeção de Dependência: O código pede o banco e o .NET entrega
    public PacienteRepository(ClinicaDbContext context)
    {
        _context = context;
    }

    public async Task<Paciente> CriarAsync(Paciente paciente)
    {
        _context.Pacientes.Add(paciente); // Marca a entidade para ser inserida no EF
        await _context.SaveChangesAsync(); // O EF gera o INSERT INTO automaticamente aqui
        return paciente;
    }

    public async Task<List<Paciente>> ObterTodosAsync()
    {
        return await _context.Pacientes.ToListAsync(); // O EF gera o SELECT * automaticamente
    }

    public async Task<Paciente?> ObterPorIdAsync(int pacienteId)
    {
        return await _context.Pacientes.FindAsync(pacienteId); // O EF gera o SELECT com WHERE automaticamente
    }

    public async Task AtualizarAsync(Paciente paciente)
    {
        _context.Pacientes.Update(paciente); // Marca a entidade como modificada no EF
        await _context.SaveChangesAsync(); // O EF gera o UPDATE automaticamente aqui
    }

    public async Task DeletarAsync(Paciente paciente)
    {
        _context.Pacientes.Remove(paciente); // Marca a entidade para remoção no EF
        await _context.SaveChangesAsync(); // O EF gera o DELETE automaticamente aqui
    }
}