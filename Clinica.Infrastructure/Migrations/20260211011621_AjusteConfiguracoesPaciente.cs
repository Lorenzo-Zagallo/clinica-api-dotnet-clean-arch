using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinica.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjusteConfiguracoesPaciente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Pacientes",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "CPF",
                table: "Pacientes",
                newName: "cpf");

            migrationBuilder.RenameColumn(
                name: "ValorConsulta",
                table: "Pacientes",
                newName: "valor_consulta");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "nome",
                table: "Pacientes",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "cpf",
                table: "Pacientes",
                newName: "CPF");

            migrationBuilder.RenameColumn(
                name: "valor_consulta",
                table: "Pacientes",
                newName: "ValorConsulta");
        }
    }
}
