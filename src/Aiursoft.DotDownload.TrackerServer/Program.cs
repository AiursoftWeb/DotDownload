using Aiursoft.CSTools.Tools;
using Aiursoft.Download.TrackerServer.Services;
using Microsoft.EntityFrameworkCore;
using static Aiursoft.WebTools.Extends;

namespace Aiursoft.Download.TrackerServer;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var app = App<Startup>(args);
        await app.StartAsync();
        await app.BootServerAsync();
        await app.WaitForShutdownAsync();
    }
}

public static class ProgramExtends
{
    public static async Task<WebApplication> BootServerAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = app.Logger;
        if (!EntryExtends.IsProgramEntry())
        {
            logger.LogWarning($"This programme was triggered by Entity framework or unit test. We should do nothing");
            return app;
        }

        var boot = services.GetRequiredService<BootService>();
        await boot.Boot();
        return app;
    }
}