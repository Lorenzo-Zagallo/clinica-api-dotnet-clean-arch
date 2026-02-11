using Clinica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clinica.Infrastructure.Configurations
{
    public class ConsultaConfiguration : IEntityTypeConfiguration<Consulta>
    {
        public void Configure(EntityTypeBuilder<Consulta> builder)
        {
            builder.ToTable("Consultas");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Tipo)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(20);
            
            builder.Property(x => x.Observacoes)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Valor)
                .HasPrecision(18, 2); // obrigatório para dinheiro (decimal)

            // --- O RELACIONAMENTO (o pulo do gato) ---
            // Dizemos: uma Consulta TEM UM Paciente, e um Paciente TEM MUITAS Consultas.
            // A chave que liga eles é PacienteId
            builder.HasOne(x => x.Paciente)
                .WithMany() // se quiser, poderia colocar .WithMany(p => p.Consultas) se tivesse a lista lá
                .HasForeignKey(x => x.PacienteId)
                .OnDelete(DeleteBehavior.Restrict); // se apagar o cliente, não apaga as consultas (segurança)
        }
    }
}