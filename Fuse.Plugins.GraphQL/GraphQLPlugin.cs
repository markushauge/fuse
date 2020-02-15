using System;
using Fuse.Plugin;
using Microsoft.AspNetCore.Hosting;

namespace Fuse.Plugins.GraphQL
{
    // ReSharper disable once InconsistentNaming
    public class GraphQLPlugin : IPlugin
    {
        private readonly IWebHost _host =
            new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://*:80")
                .UseStartup<Startup>()
                .Build();

        public void OnEnable(IPluginCollection plugins)
        {
            Console.WriteLine("GraphQL plugin enabled");
            _host.StartAsync().Wait();
        }

        public void OnDisable(IPluginCollection plugins)
        {
            Console.WriteLine("GraphQL plugin disabled");
            _host.StopAsync().Wait();
        }
    }
}
