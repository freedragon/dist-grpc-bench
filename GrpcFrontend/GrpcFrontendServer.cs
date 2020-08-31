using Grpc.Core;
using ModelLibrary.GRPC;
using System.Threading.Tasks;

namespace GrpcFrontend
{
    public class GrpcFrontendServer
    {
        private readonly Server server;

        public GrpcFrontendServer()
        {
            server = new Server
            {
                Services = { MeteoriteLandingsService.BindService(new GrpcFrontendServiceImpl()) },
                Ports = { new ServerPort("localhost", 7000, ServerCredentials.Insecure) }
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
