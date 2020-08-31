using Grpc.Core;
using ModelLibrary.GRPC;
using System.Threading.Tasks;

namespace GrpcAPI
{
    public class MeteoriteLandingServer
    {
        private readonly Server server;

        public MeteoriteLandingServer(string host, int port)
        {
            server = new Server
            {
                Services = { MeteoriteLandingsService.BindService(new MeteoriteLandingsServiceImpl()) },
                Ports = { new ServerPort(host, port, ServerCredentials.Insecure) }
            };
        }

        public void Start()
        {
            server.Start();
        }

        public async Task ShutdownAsync()
        {
            await server.ShutdownAsync();
        }
    }
}
