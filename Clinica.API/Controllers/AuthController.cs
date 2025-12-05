using Clinica.API.Services;
using Clinica.Application.DTOs;
using Clinica.Domain.Entities;
using Clinica.Domain.Interfaces;
using Clinica.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;

namespace Clinica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly TokenService _tokenService;
    private readonly ClinicaDbContext _clinicaDbContext;

    public AuthController(IUsuarioRepository usuarioRepository, TokenService tokenService, ClinicaDbContext clinicaDbContext)
    {
        _usuarioRepository = usuarioRepository;
        _tokenService = tokenService;
        _clinicaDbContext = clinicaDbContext;
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        // 1. Verifica se existe alguém com esse usuário e senha
        var usuario = await _usuarioRepository.ObterUsuarioPorLoginSenhaAsync(dto.NomeUsuario, dto.Senha);

        // 2. Se não existir, retorna 401 (Unauthorized)
        if (usuario == null) return Unauthorized("Usuário ou senha inválidos");

        // 3. Se existir, gera o Token
        var token = _tokenService.GerarToken(usuario);

        // 4. Retorna o Token para o front-end usar
        return Ok(new
        {
            usuario = usuario.NomeUsuario,
            token = token
        });
    }

    // POST: api/auth/registrar
    // (Endpoint auxiliar apenas para criarmos o primeiro usuário de teste)
    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] LoginDTO dto)
    {
        var usuario = new Usuario
        {
            NomeUsuario = dto.NomeUsuario,
            Senha = dto.Senha
        };

        _clinicaDbContext.Usuarios.Add(usuario);
        await _clinicaDbContext.SaveChangesAsync();

        return Ok("Usuário criado com sucesso!");
    }
}