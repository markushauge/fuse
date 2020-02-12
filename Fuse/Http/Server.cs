using Microsoft.AspNetCore.Hosting;

namespace Fuse.Http
{
    public static class Server
    {
        public static void Run()
        {
            new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://*:80")
                .UseStartup<Startup>()
                .Build()
                .RunAsync();
        }
    }
}
