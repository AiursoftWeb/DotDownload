using Aiursoft.Canon;
using Aiursoft.CommandFramework.Abstracts;
using Aiursoft.DotDownload.PluginFramework.Handlers.Download;
using Microsoft.Extensions.DependencyInjection;

namespace Aiursoft.DotDownload.Http;

public class Startup : IStartUp
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddTaskCanon();
        services.AddTransient<Downloader>();
    }
}
