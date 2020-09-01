using System;
using Grpc.Core;
using ModelLibrary.GRPC;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ModelLibrary.GRPC.MeteoriteLandingsService;

namespace DistributedGRPC
{
    public class GRPCClient
    {
        private readonly Channel channel;
        private readonly MeteoriteLandingsServiceClient client;

        // public GRPCClient(string host ="localhost", int port =7000)
        public GRPCClient()
        {
            string host = Environment.GetEnvironmentVariable("BenchFEHost") ?? "localhost";
            int port = Int32.Parse(Environment.GetEnvironmentVariable("BenchFEPort") ?? "6000");

            string channelHost = string.Format("{0}:{1}", host, port);

            channel = new Channel(channelHost, ChannelCredentials.Insecure);
            client = new MeteoriteLandingsServiceClient(channel);

            Console.WriteLine("gRpc client initialized. Service will be called wtih {0}", channelHost);
        }

        public async Task<string> GetSmallPayloadAsync()
        {
            return (await client.GetVersionAsync(new EmptyRequest())).ApiVersion;
        }

        public async Task<List<MeteoriteLanding>> StreamLargePayloadAsync()
        {
            List<MeteoriteLanding> meteoriteLandings = new List<MeteoriteLanding>();

            using (var response = client.GetLargePayload(new EmptyRequest()).ResponseStream)
            {
                while (await response.MoveNext())
                {
                    meteoriteLandings.Add(response.Current);
                }
            }

            return meteoriteLandings;
        }

        public async Task<IList<MeteoriteLanding>> GetLargePayloadAsListAsync()
        {
            return (await client.GetLargePayloadAsListAsync(new EmptyRequest())).MeteoriteLandings;
        }

        public async Task<string> PostLargePayloadAsync(MeteoriteLandingList meteoriteLandings)
        {
            return (await client.PostLargePayloadAsync(meteoriteLandings)).Status;
        }
    }
}
