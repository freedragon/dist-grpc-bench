### Distributed GRPC performance test (at least 2 different tiers)

# dist-grpc-bench in C# (REST API is not tested/bencharked against grpc)
Evaluating Performance of gRPC with different network configuration on Azure Virtual Networks peered.
Services will be run on container. Dockerfile will used to make different docker container images for this test.

## Build project (Modified from orignal reference - https://github.com/EmperorRXF/RESTvsGRPC)
```
dotnet build -c Release DistGRPC.sln
```

* You need to open up 3 terminal windows/sessions at least to run this project from loadhost.

## Start Grpc API which stands for main workload servicing through GRPC.
Starts the ASP.NET MVC Core REST API
```
dotnet run -p GrpcAPI -c Release
```

## Start relaying Grpc API which will call Grpc API started above
```
dotnet run -p GrpcFrontend -c Release
```

## Start benchmarking test 
Runs the benchmark on the above services
```
dotnet run -p DistGRPC -c Release
```

## Sample output after some successful bechmark test run.
```
// * Summary *

BenchmarkDotNet=v0.11.4, OS=ubuntu 16.04
Intel Xeon CPU E5-2673 v4 2.30GHz, 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=2.2.300
  [Host]     : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT


|                         Method | IterationCount |           Mean |        Error |       StdDev |         Median |
|------------------------------- |--------------- |---------------:|-------------:|-------------:|---------------:|
|       GrpcGetSmallPayloadAsync |              1 |       984.4 us |     19.23 us |     33.17 us |       983.1 us |
|    GrpcStreamLargePayloadAsync |              1 |    88,603.1 us |  2,325.24 us |  6,819.54 us |    88,687.6 us |
| GrpcGetLargePayloadAsListAsync |              1 |     6,006.8 us |    119.90 us |    247.62 us |     5,930.7 us |
|      GrpcPostLargePayloadAsync |              1 |     5,331.3 us |    267.24 us |    740.51 us |     5,118.8 us |
|       GrpcGetSmallPayloadAsync |             10 |     9,249.7 us |    182.63 us |    237.47 us |     9,218.8 us |
|    GrpcStreamLargePayloadAsync |             10 |   814,263.3 us | 13,981.98 us | 13,078.75 us |   812,633.9 us |
| GrpcGetLargePayloadAsListAsync |             10 |    57,115.1 us |    759.37 us |    673.16 us |    57,234.2 us |
|      GrpcPostLargePayloadAsync |             10 |    57,389.0 us |  1,382.84 us |  4,033.82 us |    56,931.6 us |
|       GrpcGetSmallPayloadAsync |             50 |    47,064.5 us |  1,112.53 us |    986.23 us |    46,933.2 us |
|    GrpcStreamLargePayloadAsync |             50 | 4,006,807.6 us | 46,496.14 us | 43,492.51 us | 4,007,308.1 us |
| GrpcGetLargePayloadAsListAsync |             50 |   281,100.8 us |  5,430.20 us |  5,079.41 us |   280,961.4 us |
|      GrpcPostLargePayloadAsync |             50 |   287,040.0 us |  5,730.11 us | 11,960.86 us |   285,223.7 us |
|       GrpcGetSmallPayloadAsync |            100 |    91,902.2 us |  1,823.62 us |  3,599.64 us |    92,561.9 us |
|    GrpcStreamLargePayloadAsync |            100 | 7,968,732.5 us | 65,365.74 us | 61,143.16 us | 7,978,063.2 us |
| GrpcGetLargePayloadAsListAsync |            100 |   570,279.3 us | 11,308.79 us | 13,462.31 us |   568,573.7 us |
|      GrpcPostLargePayloadAsync |            100 |   560,649.7 us |  8,762.72 us |  7,767.92 us |   562,359.9 us |
```
## Project Reference - [https://medium.com/@EmperorRXF/evaluating-performance-of-rest-vs-grpc](https://medium.com/@EmperorRXF/evaluating-performance-of-rest-vs-grpc-1b8bdf0b22da?source=https://github.com/EmperorRXF/RESTvsGRPC)

## Commandline parser references:

* https://www.nuget.org/packages/CommandLineParser/
* https://github.com/commandlineparser/commandline

