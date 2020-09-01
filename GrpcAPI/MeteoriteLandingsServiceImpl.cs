using System;
using System.Threading.Tasks;
using Grpc.Core;
using ModelLibrary.Data;
using ModelLibrary.GRPC;

namespace GrpcAPI
{
    public class MeteoriteLandingsServiceImpl : MeteoriteLandingsService.MeteoriteLandingsServiceBase
    {
        public override Task<ModelLibrary.GRPC.Version> GetVersion(EmptyRequest request, ServerCallContext context)
        {
            Console.WriteLine(">> GetVersion called: {0}", request);
            return Task.FromResult(new ModelLibrary.GRPC.Version
            {
                ApiVersion = "API Version 1.0"
            });
        }

        public override async Task GetLargePayload(EmptyRequest request, IServerStreamWriter<MeteoriteLanding> responseStream, ServerCallContext context)
        {
            Console.WriteLine(">> GetLargePayload called: {0}", request);
            foreach (var meteoriteLanding in MeteoriteLandingData.GrpcMeteoriteLandings)
            {
                await responseStream.WriteAsync(meteoriteLanding);
            }
        }

        public override Task<MeteoriteLandingList> GetLargePayloadAsList(EmptyRequest request, ServerCallContext context)
        {
            Console.WriteLine(">> GetLargePayloadAsList called: {0}", request);
            return Task.FromResult(MeteoriteLandingData.GrpcMeteoriteLandingList);
        }

        public override Task<StatusResponse> PostLargePayload(MeteoriteLandingList request, ServerCallContext context)
        {
            Console.WriteLine(">> PostLargePayload called: {0}", request);
            return Task.FromResult(new StatusResponse { Status = "SUCCESS" });
        }
    }
}
