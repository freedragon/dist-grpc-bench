using System;
using System.Threading.Tasks;

namespace GrpcFrontend
{
    class Program
    {
        static void Main(string[] args)
        {
            StartServer().GetAwaiter().GetResult();
        }

        private static async Task StartServer()
        {
            var server = new GrpcFrontendServer();
            server.Start();

            Console.WriteLine("GRPC Frontend Service Running on localhost:7000");
            Console.ReadKey();

            await server.ShutdownAsync();
        }
    }
}
