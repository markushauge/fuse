using System;
using Fuse.Plugin;
using Microsoft.AspNetCore.Hosting;

namespace Fuse.Plugins.GraphQL
{
    // ReSharper disable once InconsistentNaming
    public class GraphQLPlugin : IPlugin
    {
        private const string Url = "http://*:80";

        private readonly IWebHost _host =
            new WebHostBuilder()
                .UseKestrel()
                .UseUrls(Url)
                .UseStartup<Startup>()
                .Build();

        public void OnEnable(IPluginCollection plugins)
        {
            _host.StartAsync().Wait();
            Console.WriteLine($"[GraphQL] Now listening on: {Url}");
        }

        public void OnDisable(IPluginCollection plugins)
        {
            _host.StopAsync().Wait();
        }
    }
}
