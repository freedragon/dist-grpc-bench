# Dockerize an ASP.NET Core application
https://docs.docker.com/engine/examples/dotnetcore/

# Docker images for ASP.NET Core
https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-3.1
(Used 3.1-bionics based on Ubuntu 18.04)

```
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionics AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY aspnetapp/*.csproj ./aspnetapp/
RUN dotnet restore

# copy everything else and build app
COPY aspnetapp/. ./aspnetapp/
WORKDIR /app/aspnetapp
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionics AS runtime
WORKDIR /app
COPY --from=build /app/aspnetapp/out ./
ENTRYPOINT ["dotnet", "aspnetapp.dll"]
```

# Docker commands to build images
```
docker build -t dotnet-daks-backend:0.1 . --file Dockerfile.backend

docker build -t dotnet-daks-frontend:0.1 . --file Dockerfile.frontend
```

## Tutorial: Containerize a .NET Core app
https://docs.microsoft.com/en-us/dotnet/core/docker/build-container?tabs=windows

