# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copie o .csproj e restaure as dependências
COPY ["DiscSteam/DiscSteam.csproj", "."]
RUN dotnet restore

# Copie todo o código e faça o build
COPY . .
RUN dotnet publish -c Release -o /app

# Estágio de Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copie os arquivos publicados
COPY --from=build /app .

# Copie o arquivo config.json para o caminho esperado
COPY ["DiscSteam/config/config.json", "/app/config/config.json"]

# Defina o ponto de entrada
ENTRYPOINT ["dotnet", "DiscSteam.dll"]

# Variaveis
ENV STEAM_KEY="" 
ENV ACESS_TOKEN="" 
ENV TOKEN_BOT="" 
ENV NEW_GAMES_OF_FAMILY=0