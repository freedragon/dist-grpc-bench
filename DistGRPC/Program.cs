using BenchmarkDotNet.Running;
using System;
using CommandLine;

namespace DistributedGRPC
{
    public static class SHostSettings
    {
        public static string Host { get; set; }
        public static int Port { get; set; }
    }

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

                       SHostSettings.Host = o.Host;
                       SHostSettings.Port = o.Port;

                       Console.WriteLine("Host Settings = {0}:{1}", SHostSettings.Host, SHostSettings.Port);
                       BenchmarkRunner.Run<BenchmarkHarness>();
                   });
        }

    }
}
