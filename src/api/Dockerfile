# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia arquivos de solução e projeto para cache do restore
COPY tecnica.sln ./
COPY Tecnica/*.csproj ./Tecnica/
RUN dotnet restore tecnica.sln

# Copia todo o código
COPY . .

# Publica o projeto
WORKDIR /src/Tecnica
RUN dotnet publish -c Release -o /app /p:UseAppHost=false

# Etapa runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app .

# Porta padrão do Heroku
ENV ASPNETCORE_URLS=http://*:${PORT:-5000}
EXPOSE ${PORT:-5000}

# Comando para rodar a API
ENTRYPOINT ["dotnet", "Tecnica.dll"]
