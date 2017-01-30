using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Configuration;
using System;

namespace TheWall
{

    public class Program
    {
        public static void Main(string[] args)
        {
            // var config = new ConfigurationBuilder().AddEnvironmentVariables("ASPNETCORE_").Build();

            IWebHost host = new WebHostBuilder()
                .UseKestrel()
                // .UseConfiguration(config)
                // .UseEnvironment("Production")
                .UseEnvironment("Development")
                // .UseUrls("http://localhost:1234")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}