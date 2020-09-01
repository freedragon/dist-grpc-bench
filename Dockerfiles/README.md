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
$ docker build -t dotnet-daks-backend:0.1 . --file Dockerfile.backend
Sending build context to Docker daemon   7.68kB
Step 1/11 : FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic AS build-env
 ---> 7736faaba403
Step 2/11 : WORKDIR /app
 ---> Running in 84ef8a93e5d4
Removing intermediate container 84ef8a93e5d4
 ---> ea73d03d569b
Step 3/11 : RUN apt-get update && apt-get install -y git
 ---> Running in 799021c25c4b
Get:1 http://security.ubuntu.com/ubuntu bionic-security InRelease [88.7 kB]
Get:2 http://archive.ubuntu.com/ubuntu bionic InRelease [242 kB]
Get:3 http://security.ubuntu.com/ubuntu bionic-security/multiverse amd64 Packages [9834 B]
Get:4 http://security.ubuntu.com/ubuntu bionic-security/restricted amd64 Packages [102 kB]
Get:5 http://security.ubuntu.com/ubuntu bionic-security/main amd64 Packages [1050 kB]
Get:6 http://archive.ubuntu.com/ubuntu bionic-updates InRelease [88.7 kB]
Get:7 http://archive.ubuntu.com/ubuntu bionic-backports InRelease [74.6 kB]
Get:8 http://security.ubuntu.com/ubuntu bionic-security/universe amd64 Packages [891 kB]
Get:9 http://archive.ubuntu.com/ubuntu bionic/universe amd64 Packages [11.3 MB]
Get:10 http://archive.ubuntu.com/ubuntu bionic/multiverse amd64 Packages [186 kB]
Get:11 http://archive.ubuntu.com/ubuntu bionic/main amd64 Packages [1344 kB]
Get:12 http://archive.ubuntu.com/ubuntu bionic/restricted amd64 Packages [13.5 kB]
Get:13 http://archive.ubuntu.com/ubuntu bionic-updates/main amd64 Packages [1360 kB]
Get:14 http://archive.ubuntu.com/ubuntu bionic-updates/universe amd64 Packages [1421 kB]
Get:15 http://archive.ubuntu.com/ubuntu bionic-updates/multiverse amd64 Packages [27.4 kB]
Get:16 http://archive.ubuntu.com/ubuntu bionic-updates/restricted amd64 Packages [123 kB]
Get:17 http://archive.ubuntu.com/ubuntu bionic-backports/universe amd64 Packages [8432 B]
Get:18 http://archive.ubuntu.com/ubuntu bionic-backports/main amd64 Packages [8286 B]
Fetched 18.4 MB in 6s (3280 kB/s)
Reading package lists...
Reading package lists...
Building dependency tree...
Reading state information...
git is already the newest version (1:2.17.1-1ubuntu0.7).
0 upgraded, 0 newly installed, 0 to remove and 4 not upgraded.
Removing intermediate container 799021c25c4b
 ---> a970deb844af
Step 4/11 : RUN git clone https://github.com/freedragon/dist-grpc-bench.git
 ---> Running in 142f519c4c50
Cloning into 'dist-grpc-bench'...
Removing intermediate container 142f519c4c50
 ---> 12921c9c1a47
Step 5/11 : RUN cp -r dist-grpc-bench/* ./
 ---> Running in 21b6da4db14c
Removing intermediate container 21b6da4db14c
 ---> b0a31fd2310e
Step 6/11 : RUN dotnet restore DistGRPC.sln
 ---> Running in d04f5c5ea3ce
  Determining projects to restore...
  Restored /app/DistGRPC/DistGRPC.csproj (in 8.54 sec).
  Restored /app/GrpcFrontend/GrpcFrontend.csproj (in 8.54 sec).
  Restored /app/ModelLibrary/ModelLibrary.csproj (in 8.54 sec).
  Restored /app/GrpcAPI/GrpcAPI.csproj (in 8.54 sec).
Removing intermediate container d04f5c5ea3ce
 ---> e6403dc0308f
Step 7/11 : RUN dotnet build -c Release DistGRPC.sln -o out
 ---> Running in 2a84f9bbed63
Microsoft (R) Build Engine version 16.7.0-preview-20360-03+188921e2f for .NET
Copyright (C) Microsoft Corporation. All rights reserved.

  Determining projects to restore...
  All projects are up-to-date for restore.
  ModelLibrary -> /app/out/ModelLibrary.dll
  GrpcFrontend -> /app/out/GrpcFrontend.dll
  GrpcAPI -> /app/out/GrpcAPI.dll
  DistGRPC -> /app/out/DistGRPC.dll

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:06.41
Removing intermediate container 2a84f9bbed63
 ---> ed46ca43426b
Step 8/11 : FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic
 ---> 75d45f296822
Step 9/11 : WORKDIR /root
 ---> Running in fce421e37dc3
Removing intermediate container fce421e37dc3
 ---> 7c167d4a6872
Step 10/11 : COPY --from=build-env /app/out /root
 ---> a05640dc4aaf
Step 11/11 : ENTRYPOINT [ "dotnet", "GrpcAPI.dll", "-h", "localhost", "-p", "7777" ]
 ---> Running in 9a57bfebb2dc
Removing intermediate container 9a57bfebb2dc
 ---> cbd2db637888
Successfully built cbd2db637888
Successfully tagged dotnet-daks-backend:0.1

$ docker build -t dotnet-daks-frontend:0.1 . --file Dockerfile.frontend
Sending build context to Docker daemon   7.68kB
Step 1/11 : FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic AS build-env
 ---> 7736faaba403
Step 2/11 : WORKDIR /app
 ---> Using cache
 ---> ea73d03d569b
Step 3/11 : RUN apt-get update && apt-get install -y git
 ---> Using cache
 ---> a970deb844af
Step 4/11 : RUN git clone https://github.com/freedragon/dist-grpc-bench.git
 ---> Using cache
 ---> 12921c9c1a47
Step 5/11 : RUN cp -r dist-grpc-bench/* ./
 ---> Using cache
 ---> b0a31fd2310e
Step 6/11 : RUN dotnet restore DistGRPC.sln
 ---> Using cache
 ---> e6403dc0308f
Step 7/11 : RUN dotnet build -c Release DistGRPC.sln -o out
 ---> Using cache
 ---> ed46ca43426b
Step 8/11 : FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic
 ---> 75d45f296822
Step 9/11 : WORKDIR /root
 ---> Using cache
 ---> 7c167d4a6872
Step 10/11 : COPY --from=build-env /app/out /root
 ---> Using cache
 ---> a05640dc4aaf
Step 11/11 : ENTRYPOINT [ "dotnet", "GrpcFrontend.dll", "-f", "localhost", "-p", "9900", "-b", "backend-host", "-t", "7777"]
 ---> Running in bcffbba6c76a
Removing intermediate container bcffbba6c76a
 ---> 0088cfe8a1cd
Successfully built 0088cfe8a1cd
Successfully tagged dotnet-daks-frontend:0.1

$ docker image ls
REPOSITORY                                     TAG                 IMAGE ID            CREATED             SIZE
dotnet-daks-frontend                           0.1                 0088cfe8a1cd        6 seconds ago       272MB
dotnet-daks-backend                            0.1                 cbd2db637888        27 seconds ago      272MB
...
mcr.microsoft.com/dotnet/core/aspnet           3.1-bionic          75d45f296822        12 days ago         205MB
mcr.microsoft.com/dotnet/core/sdk              3.1-bionic          7736faaba403        12 days ago         632MB
...
```
* Sample run with images built

```
$ docker run -d --rm -p 7000:7000 --name backend dotnet-daks-backend:0.1
e8ce53e765c25527e80eb80e6c20722aee5a6cfe16f0c8ccbcddf8f6493b148b
$ docker logs backend
Host Settings = localhost:7777
GRPC MeteoriteLandingServer Running on localhost:7777
$ docker run -d --rm -p 9000:9000 --name frontend dotnet-daks-frontend:0.1
cc81da9e533db236c2cfd883315086d93971bc2546ee1ca77c9b80defb8fc0bd
$ docker logs frontend
FE Host = localhost:9900, BE Host = backend-host:7777
GRPC Frontend Service Running on localhost:9900 targetting backend-host:7777
$
```


## Tutorial: Containerize a .NET Core app
https://docs.microsoft.com/en-us/dotnet/core/docker/build-container?tabs=windows

