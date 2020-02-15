using System;
using Fuse.Plugin;
using Fuse.Plugins.GraphQL;
using HotChocolate;

namespace Fuse.Plugins.Gw2
{
    public class Gw2Plugin : IPlugin, ISchemaProvider
    {
        public class Query
        {
            public string Hello => "World";
        }

        public string Name => "GuildWars2";

        public ISchema Schema =>
            new SchemaBuilder()
                .AddQueryType<Query>()
                .Create();

        public void OnEnable(IPluginCollection pluginManager)
        {
            Console.WriteLine("[Gw2Plugin] Enabled");
        }

        public void OnDisable(IPluginCollection pluginManager)
        {
            Console.WriteLine("[Gw2Plugin] Disabled");
        }
    }
}
