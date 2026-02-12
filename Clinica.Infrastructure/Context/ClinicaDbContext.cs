using Clinica.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clinica.Infrastructure.Context;

public class ClinicaDbContext : DbContext
{
    // Construtor que recebe as configurações (ex: string de conexão) e passa para a classe base DbContext
    public ClinicaDbContext(DbContextOptions<ClinicaDbContext> options) : base(options) { }

    // Cada DbSet declarado aqui virado uma tabela no banco.
    // O nome da propriedade ("Pacientes") será o nome da tabela no SQL
    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Consulta> Consultas { get; set; }

    // Configurações detalhadas (Fluent API)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração explícita para o tipo decimal no ValorConsulta (campo de dinheiro)
        /*
        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.Id); // Define PK

            entity.Property(e => e.Nome)
                .IsRequired()         // NOT NULL
                .HasMaxLength(100);   // VARCHAR(100)

            entity.Property(p => p.ValorConsulta)
                .HasColumnType("decimal(18,2)"); // Ex: 1234567890123456.78

            entity.Property(e => e.CPF)
                .HasMaxLength(14);
        });
        */

        // Isso aplica configurações de arquivos separados (mais organizados)
        // Ele diz: "Olhe para este projeto (Assembly) onde estou agora,
        // encontre TODAS as classes que herdam de IEntityTypeConfiguration
        // e aplique elas automaticamente
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClinicaDbContext).Assembly);
            
    }
}
