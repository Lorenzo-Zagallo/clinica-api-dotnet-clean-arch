using Clinica.Domain.Entities;

namespace Clinica.Domain.Interfaces;

public interface IUsuarioRepository
{
    // Retorna o usuário se achar, ou null se não achar
    Task<Usuario?> ObterUsuarioPorLoginSenhaAsync(string nomeUsuario,  string senha);
}