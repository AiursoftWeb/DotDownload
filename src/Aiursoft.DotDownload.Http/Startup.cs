using Aiursoft.Canon;
using Aiursoft.CommandFramework.Abstracts;
using Aiursoft.Scanner;
using Microsoft.Extensions.DependencyInjection;

namespace Aiursoft.DotDownload.Http;

public class Startup : IStartUp
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddTaskCanon();
        services.AddAssemblyDependencies(typeof(Startup).Assembly);
    }
}
