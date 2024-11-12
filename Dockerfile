FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release --no-restore -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
LABEL com.docker.compose.depends_on=database:service_healthy:false
LABEL traefik.enable=true
LABEL traefik.http.routers.hfd.entrypoints=web
LABEL traefik.http.routers.hfd.rule=Host(`${BACK_DOMAIN}`)
ENTRYPOINT ["dotnet", "Hfd.Backend.dll"]
