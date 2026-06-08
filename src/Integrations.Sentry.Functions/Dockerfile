FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated10.0 AS base
WORKDIR /home/site/wwwroot
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Integrations.Sentry.Functions/Integrations.Sentry.Functions.csproj", "src/Integrations.Sentry.Functions/"]
COPY ["src/Integrations.Infrastructure/Integrations.Infrastructure.csproj", "src/Integrations.Infrastructure/"]
COPY ["src/Integrations.Core/Integrations.Core.csproj", "src/Integrations.Core/"]
COPY ["src/Integrations.Sentry.Client/Integrations.Sentry.Client.csproj", "src/Integrations.Sentry.Client/"]
RUN dotnet restore "src/Integrations.Sentry.Functions/Integrations.Sentry.Functions.csproj"
COPY . .
WORKDIR "/src/src/Integrations.Sentry.Functions"
RUN dotnet build "./Integrations.Sentry.Functions.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Integrations.Sentry.Functions.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true
