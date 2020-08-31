using System;
using System.Collections.Generic;
using CommandLine;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcFrontend
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
            var server = new GrpcFrontendServer(host, port);
            server.Start();

            Console.WriteLine("GRPC Frontend Service Running on localhost:7000");
            Console.ReadKey();

            await server.ShutdownAsync();
        }
    }
}
