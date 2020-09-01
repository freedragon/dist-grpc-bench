using System;
using System.Collections.Generic;
using CommandLine;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

//
// Reference: https://stackoverflow.com/questions/13406435/is-thread-sleeptimeout-infinite-more-efficient-than-whiletrue

namespace GrpcFrontend
{
    class Program
    {
        static readonly CancellationTokenSource cts = new CancellationTokenSource();

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
#if         false
            // Following line will cause error when the code runs within container
            Console.ReadKey();
#else
            // Task running Main is efficiently suspended (no CPU use) forever until cts is activated.
            await Task.Delay(Timeout.Infinite, cts.Token).ConfigureAwait(false);
#endif
            await server.ShutdownAsync();
        }
    }
}
