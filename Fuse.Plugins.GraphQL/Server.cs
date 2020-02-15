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
    public static class Server
    {
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

        public static IWebHost Create(IEnumerable<ISchemaProvider> schemaProviders)
        {
            return new WebHostBuilder()
                .UseKestrel()
                .UseUrls(Url)
                .ConfigureServices(services => ConfigureServices(services, schemaProviders))
                .Configure(Configure)
                .Build();
        }
    }
}