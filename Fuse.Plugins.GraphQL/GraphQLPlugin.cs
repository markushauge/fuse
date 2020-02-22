using System;
using System.Collections.Generic;
using System.Linq;
using Fuse.Plugin;

namespace Fuse.Plugins.GraphQL
{
    // ReSharper disable once InconsistentNaming
    public class GraphQLPlugin : AutoDisposingPlugin
    {
        protected override void Configure(IReadOnlyCollection<IPlugin> plugins, ICollection<IDisposable> disposables)
        {
            var schemaProviders = plugins
                .FindPlugins<IHasSchemaProviders>()
                .SelectMany(x => x.SchemaProviders)
                .ToArray();

            if (schemaProviders.Length == 0)
            {
                Console.WriteLine("[GraphQLPlugin] No schema providers available. Server will not start.");
                return;
            }

            disposables.Add(Server.Create(schemaProviders));
        }
    }
}
