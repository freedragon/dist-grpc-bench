﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Validators;
using ModelLibrary.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DistributedGRPC
{

    [MarkdownExporter, AsciiDocExporter, HtmlExporter, RPlotExporter, CsvMeasurementsExporter]
    public class BenchmarkHarness
    {
        // [Params(100, 200)]
        [Params(4, 8, 40, 200)]
        public int IterationCount;

        // Hostand Port will be configured through environment vairalbes.
        readonly GRPCClient grpcClient = new GRPCClient();

        [Benchmark]
        public async Task GrpcGetSmallPayloadAsync()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                await grpcClient.GetSmallPayloadAsync();
            }
        }

        public async Task GrpcStreamLargePayloadAsync()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                await grpcClient.StreamLargePayloadAsync();
            }
        }

        [Benchmark]
        public async Task GrpcGetLargePayloadAsListAsync()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                await grpcClient.GetLargePayloadAsListAsync();
            }
        }

        [Benchmark]
        public async Task GrpcPostLargePayloadAsync()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                await grpcClient.PostLargePayloadAsync(MeteoriteLandingData.GrpcMeteoriteLandingList);
            }
        }

#if	false
        readonly RESTClient restClient = new RESTClient();
        [Benchmark]
        public async Task RestGetSmallPayloadAsync()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                await restClient.GetSmallPayloadAsync();
            }
        }

        [Benchmark]
        public async Task RestGetLargePayloadAsync()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                await restClient.GetLargePayloadAsync();
            }
        }

        [Benchmark]
        public async Task RestPostLargePayloadAsync()
        {
            for (int i = 0; i < IterationCount; i++)
            {
                await restClient.PostLargePayloadAsync(MeteoriteLandingData.RestMeteoriteLandings);
            }
        }
#endif	// 0
    }

    public class AllowNonOptimized : ManualConfig
    {
        public AllowNonOptimized()
        {
            Add(JitOptimizationsValidator.DontFailOnError);

            Add(DefaultConfig.Instance.GetLoggers().ToArray());
            Add(DefaultConfig.Instance.GetExporters().ToArray());
            Add(DefaultConfig.Instance.GetColumnProviders().ToArray());
        }
    }
}
