using Aiursoft.Scanner;
using Aiursoft.WebTools.Models;
using System.Reflection;
using Aiursoft.AiurProtocol.Server;
using Aiursoft.Canon;
using Aiursoft.AiurProtocol;
using Aiursoft.Download.TrackerServer.Sdk.Models;
using Aiursoft.Download.TrackerServer.Sdk;

namespace Aiursoft.Download.TrackerServer
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
