using Aiursoft.Canon;
using Aiursoft.CommandFramework.Abstracts;
using Aiursoft.DotDownload.Core.Services;
using Aiursoft.Scanner;
using Microsoft.Extensions.DependencyInjection;

namespace Aiursoft.DotDownload.Http;

public class Startup : IStartUp
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient(nameof(Downloader)).ConfigurePrimaryHttpMessageHandler(() => 
        {
            return new HttpClientHandler()
            {
                AllowAutoRedirect = true,
            };
        });
        services.AddTaskCanon();
        services.AddAssemblyDependencies(typeof(Startup).Assembly);
    }
}
