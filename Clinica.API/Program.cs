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

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();
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

var app = builder.Build(); // Cria o app com as configurações feitas acima 

// 1. CONFIGURAÇÕES de Ambiente/Swagger
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Garante o redirecionamento (se configurado)
app.UseHttpsRedirection(); // Middleware que força redirecionamento HTTP -> HTTPS (mais seguro)

// 2. ROUTING - Diz ao app para usar Controllers (API REST)
app.UseRouting();

// 3. ATENÇÃO: A ORDEM AQUI IMPORTA MUITO!
app.UseAuthentication(); // <--- OBRIGATÓRIO: "Quem é você?"
// 3.1 AUTORIZAÇÃO - Diz ao app para usar Autorização e depende do ROUTING
// Middleware de Autorização (ex: políticas de acesso) - não confundir com Autenticação (login/senha)
app.UseAuthorization(); // <--- OBRIGATÓRIO: "O que você pode fazer?"

// 4. MAPPING/EXECUÇÃO - Mapeia as rotas dos Controllers (API REST) - depende do ROUTING e da AUTORIZAÇÃO
app.MapControllers(); // Mapeia os endpoints dos Controllers (API REST)

/* var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
}; */

/* app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast"); */

app.Run(); // Inicia o app e começa a ouvir requisições HTTP na porta configurada (ex: 5001 para HTTPS)

/* record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
} */