using TheWall.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

// // using MySQL.Data.EntityFrameworkCore.Extensions;
// using Microsoft.Extensions.Configuration;
// using Microsoft.AspNetCore.Hosting;
// using TheWall.Services;
// using System.IO;

namespace TheWall
{
    public class Startup
    {

        // public IConfigurationRoot Configuration { get; set; }
        // public Startup(IHostingEnvironment env)
        // {
        //     var builder = new ConfigurationBuilder()
        //         .SetBasePath(Directory.GetCurrentDirectory())
        //         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //         .AddEnvironmentVariables();
        //     Configuration = builder.Build();
        // }
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = @"Server=localhost;
            database=efwall;uid=postgres;pwd=postgres;port=3306";
            // var connection = Configuration["DbInfo:ConnectionString"];
            services.AddDbContext<TestContext>(options => options.UseNpgsql(connection));
            services.AddDistributedMemoryCache();
            services.AddSession();

            // services.Configure<NpgsqlOptions>(Configuration.GetSection("DBInfo"));

            services.AddIdentity<TestUser, IdentityRole>()
                .AddEntityFrameworkStores<TestContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();
            
            // services.AddTransient<IEmailSender, AuthMessageSender>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            app.UseSession();
            app.UseStaticFiles();
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();

            app.UseIdentity();
            
            app.UseMvc();
        }
    }

}

