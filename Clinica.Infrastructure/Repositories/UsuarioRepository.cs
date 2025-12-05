using Clinica.Domain.Entities;
using Clinica.Domain.Interfaces;
using Clinica.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Clinica.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ClinicaDbContext _context;

    public UsuarioRepository(ClinicaDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObterUsuarioPorLoginSenhaAsync(string nomeUsuario, string senha)
    {
        // Busca no banco alguém com esse nome E essa senha
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.NomeUsuario == nomeUsuario && u.Senha == senha);
    }
}