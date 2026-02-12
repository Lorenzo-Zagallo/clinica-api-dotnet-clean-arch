namespace Clinica.Application.DTOs
{
    public class ReadConsultaDTO
    {
        public int Id { get; set; }
        public DateTime DataHorario { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal Valor { get; set; }

        // Aqui está o segredo: Devolvemos o NOME do paciente,
        // para não precisar bscar em outra tabela no front.
        public int PacienteId { get; set; }
        public string NomePaciente { get; set; } = string.Empty;
    }
}