FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
MAINTAINER Asem Nurzhanova "luke@pumalo.org"

RUN git clone git@bitbucket.org:User/repo.git
# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore ./Hfd.Backend.csproj
# Build and publish a release
RUN dotnet publish -c Release --no-restore -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
LABEL com.docker.compose.depends_on=database:service_healthy:false
LABEL traefik.enable=true
LABEL traefik.http.middlewares.hfd_https.redirectscheme.scheme=https
LABEL traefik.http.routers.hfd.entrypoints=web
LABEL traefik.http.routers.hfd.rule=Host(`api.hfd.su`)
LABEL traefik.http.routers.hfd_https.tls=true
LABEL traefik.http.routers.hfd_https.tls.certresolver=le
LABEL traefik.http.routers.hfd_https.entrypoints=websecure
LABEL traefik.http.routers.hfd_https.rule=Host(`api.hfd.su`)
ENTRYPOINT ["dotnet", "Hfd.Backend.dll"]
