namespace Clinica.Domain.Entities;

public class Paciente
{
    // O EF Core entende que "Id" é a chave primária (PK) automaticamente
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal ValorConsulta { get; set; }
    public string? CPF { get; set; }
    public DateTime DataCadastro { get; set; } = DateTime.Now;
}
