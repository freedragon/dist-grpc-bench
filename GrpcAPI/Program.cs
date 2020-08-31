using System.Threading.Tasks;
using System;
using CommandLine;

namespace GrpcAPI
{
    class Program
    {
        public class Options
        {
            [Option('h', "Host", Required = true, HelpText = "Set the hostname (or IP address) of service for benchmark.")]
            public string Host { get; set; }

            [Option('p', "port", Required = true, HelpText = "Set the Port number of service for benchmark.")]
            public int Port { get; set; }
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       Console.WriteLine("Host Settings = {0}:{1}", o.Host, o.Port);
                       StartServer(o.Host, o.Port).GetAwaiter().GetResult();
                   });
        }

        private static async Task StartServer(string host, int port)
        {
            var server = new MeteoriteLandingServer(host, port);
            server.Start();

            Console.WriteLine("GRPC MeteoriteLandingServer Running on localhost:6000");
            Console.ReadKey();

            await server.ShutdownAsync();
        }
    }
}
