# Etapa 1: Build (Compilação)
# Usamos a imagem do SDK para ter as ferramentas de build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia os arquivos de projeto (csproj) e restaura as dependências
# Isso é feito separadamente para aproveitar o cache do Docker
COPY ["Clinica.API/Clinica.API.csproj", "Clinica.API/"]
COPY ["Clinica.Application/Clinica.Application.csproj", "Clinica.Application/"]
COPY ["Clinica.Domain/Clinica.Domain.csproj", "Clinica.Domain/"]
COPY ["Clinica.Infrastructure/Clinica.Infrastructure.csproj", "Clinica.Infrastructure/"]

RUN dotnet restore "Clinica.API/Clinica.API.csproj"

# Copia todo o resto do código
COPY . .

# Compila e publica a versão final (Release)
WORKDIR "/src/Clinica.API"
RUN dotnet publish "Clinica.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Runtime (Execução)
# Usamos uma imagem mais leve, só com o necessário para rodar (sem SDK)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expõe as portas que a API usa (http e https)
EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "Clinica.API.dll"]