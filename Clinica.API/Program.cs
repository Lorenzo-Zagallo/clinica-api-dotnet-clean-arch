using System.Text;
using Clinica.API.Services;
using Clinica.Application.Interfaces;
using Clinica.Application.Services;
using Clinica.Domain.Interfaces;
using Clinica.Infrastructure.Context;
using Clinica.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Adição dos serviços no container

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ClinicaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Vida útil "Scoped": cria uma instância nova a cada requisição HTTP
// Injeção de dependência dos repositórios e serviços criados
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<IPacienteService, PacienteService>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<TokenService>();

builder.Services.AddControllers(); // Adiciona suporte a Controllers (API REST)

// 1. CONFIGURAÇAO DO JWT (Ensinando a API a validar o token)
var key = Encoding.ASCII.GetBytes("MinhaChaveSuperSecretaDeDesenvolvimento123!"); // A MESMA CHAVE DO TokenService

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwt =>
{
    jwt.RequireHttpsMetadata = false;
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// 2. CONFIGURAÇÃO DO SWAGGER (Adicionando o botão de Cadeado)
builder.Services.AddSwaggerGen(swag =>
{
    swag.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT aqui"
    });

    swag.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// (Resto do código: build, services containers)

// O Backend precisa dizer: "Eu aceito conversas vindas da porta 4200". Isso se chama CORS.
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // A porta do Front end
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build(); // Cria o app com as configurações feitas acima 

// --- BLOCO DE MIGRAÇÃO COM RETRY ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<ClinicaDbContext>();

    try
    {
        // Tenta conectar 10 vezes (esperando 3 segundos entre cada tentativa)
        // Isso dá 30 segundos para o SQL Server acordar
        var retryCount = 10;
        var currentRetry = 0;

        while (currentRetry < retryCount)
        {
            currentRetry++;
            logger.LogInformation($"Tentativa {currentRetry} de {retryCount}: Tentando criar/conectar ao banco...");

            try
            {
                // MUDANÇA AQUI:
                // Removemos o "CanConnect". 
                // O Migrate() tenta conectar. Se o banco não existir, ele CRIA o banco ClinicaDB sozinho.
                // Se o servidor estiver offline, ele gera exceção e cai no catch abaixo.
                context.Database.Migrate();

                logger.LogInformation("✅ Sucesso! Banco ClinicaDB criado e migrações aplicadas!");
                break; // Sai do loop
            }
            catch (Exception ex)
            {
                // Se o SQL Server ainda estiver subindo, vai dar erro de conexão.
                // Se o banco não existir, o Migrate resolveria, então o erro aqui é de conexão mesmo.
                logger.LogWarning($"⚠️ Ainda não foi possível conectar: {ex.Message}");

                if (currentRetry < retryCount)
                {
                    logger.LogInformation("⏳ Aguardando 5 segundos...");
                    System.Threading.Thread.Sleep(5000);
                }
            }
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erro crítico ao criar o banco de dados.");
    }
}
// --------------------------------------------------

// 1. CONFIGURAÇÕES de Ambiente/Swagger
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

// Garante o redirecionamento (se configurado)
app.UseHttpsRedirection(); // Middleware que força redirecionamento HTTP -> HTTPS (mais seguro)

// 2. ROUTING - Diz ao app para usar Controllers (API REST)
app.UseRouting();

// Agora ative o CORS antes do UseAuthentication
app.UseCors("PermitirAngular");

// 3. ATENÇÃO: A ORDEM AQUI IMPORTA MUITO!
app.UseAuthentication(); // <--- OBRIGATÓRIO: "Quem é você?"
// 3.1 AUTORIZAÇÃO - Diz ao app para usar Autorização e depende do ROUTING
// Middleware de Autorização (ex: políticas de acesso) - não confundir com Autenticação (login/senha)
app.UseAuthorization(); // <--- OBRIGATÓRIO: "O que você pode fazer?"

// 4. MAPPING/EXECUÇÃO - Mapeia as rotas dos Controllers (API REST) - depende do ROUTING e da AUTORIZAÇÃO
app.MapControllers(); // Mapeia os endpoints dos Controllers (API REST)

app.Run(); // Inicia o app e começa a ouvir requisições HTTP na porta configurada (ex: 5001 para HTTPS)