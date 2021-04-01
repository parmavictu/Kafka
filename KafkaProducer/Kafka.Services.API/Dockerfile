FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Kafka.Services.API/Kafka.Services.API.csproj", "Kafka.Services.API/"]
RUN dotnet restore "Kafka.Services.API/Kafka.Services.API.csproj"
COPY . .
WORKDIR "/src/Kafka.Services.API"
RUN dotnet build "Kafka.Services.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Kafka.Services.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Kafka.Services.API.dll"]