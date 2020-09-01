using BenchmarkDotNet.Running;
using System;
using CommandLine;

namespace DistributedGRPC
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
                       Environment.SetEnvironmentVariable("BenchFEHost", o.Host);
                       Environment.SetEnvironmentVariable("BenchFEPort", string.Format("{0}", o.Port));

                       Console.WriteLine("Host Settings = {0}:{1}", o.Host, o.Port);

                       BenchmarkRunner.Run<BenchmarkHarness>();
                   });
        }

    }
}
