# Imagem base do runtime ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

# Imagem SDK para build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia apenas os arquivos necessários para restore
COPY *.sln ./
COPY Tecnica/*.csproj ./Tecnica/

# Restaura dependências
RUN dotnet restore Tecnica/Tecnica.csproj

# Copia todo o código
COPY . .

# Build da aplicação
WORKDIR /src/Tecnica
RUN dotnet build -c Release -o /app/build

# Publica aplicação
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Imagem final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Porta padrão para rodar localmente
ENV PORT=5000

# Configuração exigida pelo Heroku
ENV ASPNETCORE_URLS=http://*:$PORT
CMD ["dotnet", "Tecnica.dll"]
