using HotChocolate;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Fuse
{
    public class Startup
    {
        public class Query
        {
            public string Hello => "World";
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGraphQL(sp =>
                new SchemaBuilder()
                    .AddQueryType<Query>()
                    .AddServices(sp)
                    .Create());
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseGraphQL();
            app.UsePlayground();
        }
    }
}
