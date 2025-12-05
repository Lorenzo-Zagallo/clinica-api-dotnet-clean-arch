namespace Clinica.Application.DTOs;

public class CriarPacienteDTO
{
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public decimal ValorConsulta { get; set; }
}