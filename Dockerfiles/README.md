# Dockerize an ASP.NET Core application
https://docs.docker.com/engine/examples/dotnetcore/

# Docker images for ASP.NET Core
https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-3.1

* Used 3.1-bionics based on Ubuntu 18.04.

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
docker build -t dotnet-daks-backend:0.1 . --file Dockerfile.backend
docker build -t dotnet-daks-frontend:0.1 . --file Dockerfile.frontend

docker run -d --name frontend dotnet-daks-frontend:0.1
docker run -d --name backend dotnet-daks-backend:0.1
```
* Sample run with images built

```
$ docker run -d --name backend dotnet-daks-backend:0.1
e8ce53e765c25527e80eb80e6c20722aee5a6cfe16f0c8ccbcddf8f6493b148b
$ docker logs backend
Host Settings = localhost:7777
GRPC MeteoriteLandingServer Running on localhost:7777
$ docker run -d --name frontend dotnet-daks-frontend:0.1
cc81da9e533db236c2cfd883315086d93971bc2546ee1ca77c9b80defb8fc0bd
$ docker logs frontend
FE Host = localhost:9900, BE Host = backend-host:7777
GRPC Frontend Service Running on localhost:9900 targetting backend-host:7777
$
```


## Tutorial: Containerize a .NET Core app
https://docs.microsoft.com/en-us/dotnet/core/docker/build-container?tabs=windows

