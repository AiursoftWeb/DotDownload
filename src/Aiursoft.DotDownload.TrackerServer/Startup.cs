using Aiursoft.Scanner;
using Aiursoft.WebTools.Models;
using System.Reflection;
using Aiursoft.AiurProtocol.Server;
using Aiursoft.Canon;
using Aiursoft.DotDownload.TrackerServer.Sdk;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace Aiursoft.DotDownload.TrackerServer
{
    public class Startup : IWebStartup
    {
        public void ConfigureServices(IConfiguration configuration, IWebHostEnvironment environment, IServiceCollection services)
        {
            services.AddTaskCanon();
            services.AddScannedDependencies();
            services.AddTrackerAccess();

            services
                .AddControllers()
                .AddApplicationPart(Assembly.GetExecutingAssembly())
                .AddAiurProtocol();
        }

        public void Configure(WebApplication app)
        {
            app.UseWebSockets();
            app.UseRouting();
            app.MapDefaultControllerRoute();
        }
    }
}
