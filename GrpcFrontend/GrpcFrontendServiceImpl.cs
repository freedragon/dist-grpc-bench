using System;
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

        public GrpcFrontendServiceImpl(string behost, int beport)
        {
            string channelhost = string.Format("{0}:{1}", behost, beport);
            channel = new Channel(channelhost, ChannelCredentials.Insecure);
            client = new MeteoriteLandingsServiceClient(channel);
        }

        public override async Task<ModelLibrary.GRPC.Version> GetVersion(EmptyRequest request, ServerCallContext context)
        {
            // return Task.FromResult( (await client.GetVersionAsync(new EmptyRequest())).ApiVersion );
            // return Task.FromResult( (await client.GetVersionAsync(request)));
            Console.WriteLine("<><> GetVersion relay request received");
            return  (await client.GetVersionAsync(request));
        }

        public override async Task GetLargePayload(EmptyRequest request, IServerStreamWriter<MeteoriteLanding> responseStream, ServerCallContext context)
        {
            Console.WriteLine("<><> GetLargePayload relay request received");
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
            Console.WriteLine("<><> GetLargePayloadAsList relay request received");
            // return Task.FromResult( (MeteoriteLandingList)(await client.GetLargePayloadAsListAsync(request)).MeteoriteLandings );
            return (await client.GetLargePayloadAsListAsync(request));
        }

        public override async Task<StatusResponse> PostLargePayload(MeteoriteLandingList request, ServerCallContext context)
        {
            Console.WriteLine("<><> PostLargePayload relay request received");
            // return Task.FromResult( (StatusResponse)(await client.PostLargePayloadAsync(request)).Status );
            return (await client.PostLargePayloadAsync(request));
        }
    }
}
