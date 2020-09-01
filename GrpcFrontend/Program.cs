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
            [Option('f', "F.E. Host", Required = true, HelpText = "Set the hostname (or IP address) of Frontend service.")]
            public string FEHost { get; set; }

            [Option('p', "F.E. Port", Required = true, HelpText = "Set the Port number of Frontend service.")]
            public int FEPort { get; set; }

            [Option('b', "B.E. Host", Required = true, HelpText = "Set the hostname (or IP address) of Backend service.")]
            public string BEHost { get; set; }

            [Option('t', "B.E. Port", Required = true, HelpText = "Set the Port number of Backend service.")]
            public int BEPort { get; set; }
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       Console.WriteLine("FE Host = {0}:{1}, BE Host = {2}:{3}", o.FEHost, o.FEPort, o.BEHost, o.BEPort);
                       StartServer(o.FEHost, o.FEPort, o.BEHost, o.BEPort).GetAwaiter().GetResult();
                   });
        }

        private static async Task StartServer(string fehost, int feport, string behost, int beport)
        {
            var server = new GrpcFrontendServer(fehost, feport, behost, beport);
            server.Start();

            Console.WriteLine("GRPC Frontend Service Running on localhost:7000");
            Console.ReadKey();

            await server.ShutdownAsync();
        }
    }
}
