using Clinica.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clinica.Infrastructure.Context;

public class ClinicaDbContext : DbContext
{
    // Construtor que recebe as configurações (ex: string de conexão) e passa para a classe base DbContext
    public ClinicaDbContext(DbContextOptions<ClinicaDbContext> options) : base(options) { }

    // Aqui dizemos: "Quero uma tabela de Pacientes baseada na classe Paciente"
    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    // Configurações adicionais do modelo de dados podem ser feitas aqui se necessário
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração explícita para o tipo decimal no ValorConsulta (campo de dinheiro)
        modelBuilder.Entity<Paciente>()
            .Property(p => p.ValorConsulta)
            .HasColumnType("decimal(18,2)"); // Ex: 1234567890123456.78
    }
}
