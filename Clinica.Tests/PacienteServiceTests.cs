using Clinica.Application.DTOs;
using Clinica.Application.Services;
using Clinica.Domain.Entities;
using Clinica.Domain.Interfaces;
using Moq;

namespace Clinica.Tests;

public class PacienteServiceTests
{
    // O Mock é um "Ator" que vai fingir ser o Repositório
    private readonly Mock<IPacienteRepository> _pacienteRepositoryMock;
    
    // O Service é quem queremos testar de verdade
    private readonly PacienteService _pacienteService;

    public PacienteServiceTests()
    {
        // 1. Preparamos o cenário
        _pacienteRepositoryMock = new Mock<IPacienteRepository>();

        // Injetamos o repositório falso dentro do serviço
        _pacienteService = new PacienteService(_pacienteRepositoryMock.Object);
    }

    [Fact] // [Fact] significa que isso é um teste que deve ser rodado
    public async Task CadastrarPaciente_DeveRetornarSucesso_QuandoDadosForemValidos()
    {
        // ARRANGE (Preparação)
        // Criamos os dados que vamos enviar (o DTO)
        var dto = new CriarPacienteDTO
        {
            Nome = "Teste Unitário",
            CPF = "111.111.111-11",
            ValorConsulta = 100
        };

        // Ensinamos o Mock: "Quando alguém chamar o método CriarAsync, retorne um Paciente com ID 1"
        _pacienteRepositoryMock.Setup(x => x.CriarAsync(It.IsAny<Paciente>()))
            .ReturnsAsync((Paciente p) =>
            {
                p.Id = 1; // Simulamos que o banco gerou o ID 1
                return p;
            });
        
        // ACT (Ação)
        // Executamos o método que queremos testar
        var resultado = await _pacienteService.CadastrarPacienteAsync(dto);
        
        // ASSERT (Verificação)
        // Verificamos se o resultado foi o esperado
        Assert.NotNull(resultado); // Não pode ser nulo
        Assert.Equal(1, resultado.Id); // O ID deve ser 1
        Assert.Equal(dto.Nome, resultado.Nome); // O nome deve ser o mesmo que enviamos

        // Verificamos se o Service realmente chamou o Repositóruio 1 vez
        _pacienteRepositoryMock.Verify(x => x.CriarAsync(It.IsAny<Paciente>()), Times.Once);
    }

    [Fact]
    public async Task CadastrarPaciente_DeveLancarErro_QuandoValorForNegativo()
    {
        // ARRANGE
        var dto = new CriarPacienteDTO
        {
            Nome = "Teste Negativo",
            ValorConsulta = -50 // Valor inválido
        };

        // ACT && ASSERT
        // Esperamos que o código lance uma Exception
        await Assert.ThrowsAsync<Exception>(() => _pacienteService.CadastrarPacienteAsync(dto));
    }
}