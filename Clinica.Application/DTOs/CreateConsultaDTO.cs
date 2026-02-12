using System.ComponentModel.DataAnnotations;

namespace Clinica.Application.DTOs
{
    public class CreateConsultaDTO
    {
        [Required]
        public int PacienteId { get; set; } // Quem é o paciente?

        [Required]
        public DateTime DataHorario { get; set; } // Quando?

        public string Tipo { get; set; } = "Rotina"; // ex: Retorno, Exame, Rotina

        public string? Observacoes { get; set; }

        [Required]
        public decimal Valor { get; set; } // Quanto vai custar?
    }
}