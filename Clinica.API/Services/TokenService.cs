using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Clinica.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Clinica.API.Services;

public class TokenService
{
    public string GerarToken(Usuario usuario)
    {
        // 1. O Manipulador (A "Máquina de Crachás")
        // Esta classe é nativa do pacote que instalamos. Ela sabe como criar, ler e validar tokens JWT.
        // É ela quem vai fazer todo o trabalho pesado.
        var tokenHandler = new JwtSecurityTokenHandler();

        // 2. A Chave Secreta (O "Carimbo Oficial")
        // Esta string longa é a senha da sua API. O token é assinado com ela.
        // Transformamos a string em um array de bytes porque o algoritmo de criptografia exige números (bytes), não texto.
        // IMPORTANTE: Se alguém descobrir essa frase, consegue criar tokens falsos e invadir seu sistema.
        var key = Encoding.ASCII.GetBytes("MinhaChaveSuperSecretaDeDesenvolvimento123!");

        // 3. O Descritor (O "Formulário de Preenchimento")
        // Aqui nós definimos como o token será. É a configuração antes de "imprimir".
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            // 4. Subject (O "Dono do Crachá")
            // Claims são as informações gravadas no token.
            // Aqui estamos dizendo: "Dentro deste token, grave que o Nome do usuário é o usuario.NomeUsuario".
            // Quando o front-end mandar esse token de volta, saberemos quem ele é só lendo isso.
            Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, usuario.NomeUsuario)
                }),

            // 5. Expires (A "Validade")
            // O token tem vida útil. Aqui definimos 8 horas.
            // Se o usuário tentar usar esse token daqui a 9 horas, a API rejeita automaticamente (erro 401).
            // Usamos UtcNow para evitar problemas de fuso horário (servidor no Brasil vs EUA).
            Expires = DateTime.UtcNow.AddHours(8),

            // 6. SigningCredentials (A "Assinatura Criptografada")
            // Aqui dizemos: "Assine este token usando minha chave (key) e o algoritmo HmacSha256".
            // O HmacSha256 é um padrão de indústria super seguro. Ele garante que o token não foi alterado.
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        // 7. Criar o Objeto Token
        // O handler pega o formulário (descriptor) e cria o objeto token na memória.
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // 8. Serializar (Transformar em String)
        // O objeto token é complexo. O método WriteToken transforma ele naquela string gigante
        // cheia de letras e números ("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...") para trafegar na internet.
        return tokenHandler.WriteToken(token);
    }
}