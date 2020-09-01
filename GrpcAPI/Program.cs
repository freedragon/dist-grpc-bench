using System;
using System.Collections.Generic;
using CommandLine;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

//
// Reference: https://stackoverflow.com/questions/13406435/is-thread-sleeptimeout-infinite-more-efficient-than-whiletrue

namespace GrpcAPI
{
    class Program
    {
        static readonly CancellationTokenSource cts = new CancellationTokenSource();

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
