using System.Threading.Tasks;
using Grpc.Core;
using ModelLibrary.GRPC;
using System.Collections.Generic;
// using System.Threading.Tasks;
using static ModelLibrary.GRPC.MeteoriteLandingsService;

namespace GrpcFrontend
{
    /*
         GrpcClient call proxy
     */
    public class GrpcFrontendServiceImpl : MeteoriteLandingsService.MeteoriteLandingsServiceBase
    {
        private readonly Channel channel;
        private readonly MeteoriteLandingsServiceClient client;

        public GrpcFrontendServiceImpl()
        {
            channel = new Channel("localhost:6000", ChannelCredentials.Insecure);
            client = new MeteoriteLandingsServiceClient(channel);
        }

        public override async Task<Version> GetVersion(EmptyRequest request, ServerCallContext context)
        {
            // return Task.FromResult( (await client.GetVersionAsync(new EmptyRequest())).ApiVersion );
            // return Task.FromResult( (await client.GetVersionAsync(request)));
            return  (await client.GetVersionAsync(request));
        }

        public override async Task GetLargePayload(EmptyRequest request, IServerStreamWriter<MeteoriteLanding> responseStream, ServerCallContext context)
        {
            /*
            foreach (var meteoriteLanding in MeteoriteLandingData.GrpcMeteoriteLandings)
            {
                await responseStream.WriteAsync(meteoriteLanding);
            }
             */
            List<MeteoriteLanding> meteoriteLandings = new List<MeteoriteLanding>();

            using (var response = client.GetLargePayload(request).ResponseStream)
            {
                while (await response.MoveNext())
                {
                    meteoriteLandings.Add(response.Current);
                }
            }
            // return meteoriteLandings;
        }

        public override async Task<MeteoriteLandingList> GetLargePayloadAsList(EmptyRequest request, ServerCallContext context)
        {
            // return Task.FromResult( (MeteoriteLandingList)(await client.GetLargePayloadAsListAsync(request)).MeteoriteLandings );
            return (await client.GetLargePayloadAsListAsync(request));
        }

        public override async Task<StatusResponse> PostLargePayload(MeteoriteLandingList request, ServerCallContext context)
        {
            // return Task.FromResult( (StatusResponse)(await client.PostLargePayloadAsync(request)).Status );
            return (await client.PostLargePayloadAsync(request));
        }
    }
}
