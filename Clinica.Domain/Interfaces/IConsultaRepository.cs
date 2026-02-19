namespace Clinica.Domain.Interfaces;

public interface IConsultaRepository
{
    Task<decimal> FaturamentoTotal();
    Task<int> TotalConsultas();
}