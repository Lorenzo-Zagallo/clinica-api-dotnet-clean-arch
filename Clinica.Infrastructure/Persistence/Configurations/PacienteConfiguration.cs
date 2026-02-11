using Clinica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clinica.Infrastructure.Configurations
{
    public class PacienteConfiguration : IEntityTypeConfiguration<Paciente>
    {
        public void Configure(EntityTypeBuilder<Paciente> builder)
        {
            // 1. Nome da tabela
            builder.ToTable("Pacientes");

            // 2. Chave Primária
            builder.HasKey(p => p.Id);

            // 3. Propriedades
            builder.Property(p => p.Nome)
                .HasColumnName("nome")
                .IsRequired()                   // Not Null
                .HasMaxLength(100)              // Varchar(100)
                .HasColumnType("varchar(100)"); // Força varchar no SQL Server

            builder.Property(p => p.CPF)
                .HasColumnName("cpf")
                .IsRequired()
                .HasMaxLength(14) // 000.000.000-00
                .HasColumnType("varchar(14)");

            builder.Property(p => p.ValorConsulta)
                .HasColumnName("valor_consulta")
                .IsRequired()
                .HasPrecision(18, 2) // configurando valor decimal (precisa de precisão) - 18 dígitos, 2 decimais - Ex: 1234567890123456.78
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.DataCadastro)
                .HasColumnName("data_cadastro")
                .IsRequired();
        }
    }
}