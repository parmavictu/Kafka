### Estágio 1 - Obter o source e gerar o Build ###
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS dotnet-builder
RUN pwsh -Command Write-Host "Teste: Gerando uma nova imagem Docker e testando o PowerShell Core"
WORKDIR /app
COPY ./KafkaConsumerStudent/ .
RUN ls
RUN dotnet publish ./Kafka.Services.Consumer.Student/Kafka.Services.Consumer.Student.csproj -o /app/publish 

### Estágio 2 - Subir a aplicação através dos binários ###
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=dotnet-builder /app/publish .
ENTRYPOINT ["dotnet", "Kafka.Services.Consumer.Student.dll"]
