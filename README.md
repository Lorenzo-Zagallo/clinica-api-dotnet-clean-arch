
# 🏥 Clínica API - Clean Architecture & .NET 10

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![EF Core](https://img.shields.io/badge/EF%20Core-Code%20First-blue)](https://docs.microsoft.com/ef/core/)
[![License](https://img.shields.io/badge/License-MIT-green)](LICENSE)

API RESTful moderna desenvolvida para gerenciamento de pacientes e autenticação de usuários, demonstrando a aplicação prática de princípios de **Arquitetura Limpa (Clean Architecture)** e **SOLID**.

O projeto moderniza um sistema legado (Web Forms), migrando para a estrutura mais recente, escalável, testável e performática do ecossistema Microsoft.

## 🚀 Tecnologias & Práticas

- **.NET 10 SDK**: Utilizando a versão mais recente e performática da plataforma (LTS).
- **Clean Architecture**: Separação clara de responsabilidades (Domain, Application, Infrastructure, API).
- **Entity Framework Core**: ORM para manipulação de dados com abordagem *Code-First*.
- **SQL Server**: Banco de dados relacional robusto.
- **JWT (JSON Web Token)**: Autenticação e segurança de endpoints via Bearer Token.
- **Swagger UI**: Documentação interativa e testes de API.
- **Injeção de Dependência**: Desacoplamento de componentes nativo do .NET.

## 🏗️ Arquitetura do Projeto

A solução foi dividida em camadas para garantir a manutenção e testabilidade:

1.  **Domain**: O coração do sistema. Contém as Entidades (`Paciente`, `Usuario`) e Interfaces de Repositório. Zero dependências externas.
2.  **Application**: Casos de uso e regras de negócio. Contém os DTOs (`LoginDTO`, `CriarPacienteDTO`), Interfaces de Serviços e Implementações (`PacienteService`).
3.  **Infrastructure**: Implementação técnica. Contexto do Banco de Dados (`DbContext`), Migrations e Repositórios concretos (`PacienteRepository`).
4.  **API**: A porta de entrada. Controllers RESTful, Configuração de JWT e Injeção de Dependência (`Program.cs`).

## ⚙️ Como Executar

### Pré-requisitos
- .NET 10 SDK
- SQL Server (LocalDB ou Docker)

### Passo a Passo
1. Clone o repositório:
   ```bash
   git clone https://github.com/seu-usuario/clinica-api-dotnet-clean-arch.git
    ```

2.  Configure a string de conexão no `appsettings.json` (se necessário).

3.  Aplique as migrações para criar o banco de dados:

    ```bash
    dotnet ef database update --project Clinica.Infrastructure --startup-project Clinica.API
    ```

4.  Execute a API:

    ```bash
    dotnet run --project Clinica.API
    ```

5.  Acesse o Swagger em: `http://localhost:5xxx/swagger`

## 🔒 Autenticação

O sistema possui endpoints protegidos. Para testar:

1.  Crie um usuário na rota `POST /api/auth/registrar`.
2.  Faça login em `POST /api/auth/login` para receber o **Token JWT**.
3.  No Swagger, clique no cadeado 🔓 e insira o token (o prefixo `Bearer` é adicionado automaticamente).

-----

Desenvolvido por **Lorenzo Zagallo** 👨‍💻
