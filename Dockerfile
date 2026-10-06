# Compila o código com publish para pasta /app/publish
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src
# Copia o arquivo csproj (onde define as dependências para então instalar as mesmas)
COPY LudoVault.csproj ./
# Restaura pacotes dotnet
RUN dotnet restore
# Copia o restante dos arquivos do projeto
COPY . .
# COmpila e publica a aplicação
RUN dotnet publish LudoVault.csproj -c Release -o /app/publish

# A partir da compilação, executa o dll criado e expoê a porta
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT [ "dotnet", "LudoVault.dll" ]