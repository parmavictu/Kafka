### Estágio 1 - Obter o source e gerar o Build ###
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS dotnet-builder
WORKDIR /app
COPY ./KafkaProducer/ .
RUN ls
RUN dotnet publish ./Kafka.Services.API/Kafka.Services.API.csproj -o /app/publish 

### Estágio 2 - Subir a aplicação através dos binários ###
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
ENV ASPNETCORE_URLS http://*:8090
EXPOSE 8090
COPY --from=dotnet-builder /app/publish .
ENTRYPOINT ["dotnet", "Kafka.Services.API.dll"]
