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
                // .UseEnvironment("Development")
                // .UseUrls("https://localhost:44377", "http://localhost:5000")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}