using Aiursoft.Canon;
using Aiursoft.CommandFramework.Abstracts;
using Aiursoft.DotDownload.PluginFramework.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Aiursoft.DotDownload.Calendar;

public class Startup : IStartUp
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTaskCanon();
    }
}
