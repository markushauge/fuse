using System;
using System.Collections.Generic;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Subscriptions;
using HotChocolate.Stitching;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Fuse.Plugins.GraphQL
{
    public class Server : IDisposable
    {
        private readonly IWebHost _host;
        private const string Url = "http://*:80";

        private static void ConfigureServices(IServiceCollection services, IEnumerable<ISchemaProvider> schemaProviders)
        {
            services
                .AddStitchedSchema(builder =>
                {
                    foreach (var schemaProvider in schemaProviders)
                    {
                        builder.AddSchema(schemaProvider.Name, schemaProvider.Schema);
                    }
                })
                .AddGraphQLSubscriptions();
        }

        private static void Configure(IApplicationBuilder app)
        {
            app.UseGraphQL("/graphql");
            app.UsePlayground("/graphql", "/graphql");
        }

        public static Server Create(IEnumerable<ISchemaProvider> schemaProviders) =>
            new Server(
                new WebHostBuilder()
                    .UseKestrel()
                    .UseUrls(Url)
                    .ConfigureServices(services => ConfigureServices(services, schemaProviders))
                    .Configure(Configure)
                    .Build()
            );

        public Server(IWebHost host)
        {
            _host = host;
            _host.StartAsync().Wait();
        }

        public void Dispose()
        {
            _host.StopAsync().Wait();
        }
    }
}