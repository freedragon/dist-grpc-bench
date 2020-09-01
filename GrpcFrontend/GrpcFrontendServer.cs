using Grpc.Core;
using ModelLibrary.GRPC;
using System.Threading.Tasks;

namespace GrpcFrontend
{
    public class GrpcFrontendServer
    {
        private readonly Server server;

        public GrpcFrontendServer(string fehost, int feport, string behost, int beport)
        {
            server = new Server
            {
                Services = { MeteoriteLandingsService.BindService(new GrpcFrontendServiceImpl(behost, beport)) },
                Ports = { new ServerPort(fehost, feport, ServerCredentials.Insecure) }
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
