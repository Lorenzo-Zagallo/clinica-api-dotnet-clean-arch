namespace Clinica.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty; // Em produção, usaríamos Hash, mas vamos manter simples por enquanto
}
