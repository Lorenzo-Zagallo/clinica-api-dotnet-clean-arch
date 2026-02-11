namespace Clinica.Domain.Entities
{
    public class Consulta
    {
        public int Id { get; set; }

        // --- Relacionamento (Chave Estrangeira) ---
        public int PacienteId { get; set; } // A coluna no banco (int)
        public Paciente? Paciente { get; set; } // O objeto para navegação no código

        // --- Dados da Consulta ---
        public DateTime DataHorario { get; set; }
        public string Tipo { get; set; } = string.Empty; // ex: "Primeira Vez", "Retorno", "Exame"
        public string Status { get; set; } = "Agendado"; // ex: "Agendado", "Concluido", "Cancelado"
        public string? Observacoes { get; set; }

        // O valor cobrado (importante para o Dashboard)
        public decimal Valor { get; set; }
    }
}