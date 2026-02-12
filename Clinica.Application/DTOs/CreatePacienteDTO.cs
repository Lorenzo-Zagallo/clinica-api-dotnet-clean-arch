namespace Clinica.Application.DTOs;

public class CreatePacienteDTO
{
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public decimal ValorConsulta { get; set; }
}