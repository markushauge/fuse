using System;
using System.Linq;
using Fuse.Plugin;
using Microsoft.AspNetCore.Hosting;

namespace Fuse.Plugins.GraphQL
{
    // ReSharper disable once InconsistentNaming
    public class GraphQLPlugin : IPlugin
    {
        private IWebHost _host = null!;

        public void OnEnable(IPluginCollection plugins)
        {
            var schemaProviders = plugins
                .FindPlugins<IHasSchemaProviders>()
                .SelectMany(x => x.SchemaProviders)
                .ToArray();

            if (schemaProviders.Length == 0)
            {
                Console.WriteLine("[GraphQLPlugin] No schema providers available");
            }

            _host = Server.Create(schemaProviders);
            _host.StartAsync().Wait();
            Console.WriteLine($"[GraphQLPlugin] Enabled");
        }

        public void OnDisable(IPluginCollection plugins)
        {
            _host.StopAsync().Wait();
            Console.WriteLine($"[GraphQLPlugin] Disabled");
        }
    }
}
