using Clinica.Domain.Interfaces;
using Clinica.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Clinica.Infrastructure.Repositories;

public class ConsultaRepository : IConsultaRepository
{
    private readonly ClinicaDbContext _context;

    public ConsultaRepository(ClinicaDbContext context)
    {
        _context = context;
    }

    public async Task<decimal> FaturamentoTotal()
    {
        return await _context.Consultas
            .Where(c => c.Status != "Cancelado")
            .SumAsync(c => c.Valor);
    }

    public async Task<int> TotalConsultas()
    {
        return await _context.Consultas.CountAsync();
    }
}