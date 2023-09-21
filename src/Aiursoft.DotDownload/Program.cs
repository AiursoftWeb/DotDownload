using Aiursoft.DotDownload.PluginFramework;
using System.CommandLine;
using System.Reflection;
using Aiursoft.CommandFramework.Extensions;
using Aiursoft.DotDownload.Http;

namespace Aiursoft.DotDownload;

public class Program
{
    public static async Task Main(string[] args)
    {
        var descriptionAttribute = (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;

        var program = new RootCommand(descriptionAttribute ?? "Unknown usage.")
            .AddGlobalOptions()
            .AddPlugins(
                new HttpPlugin()
            );

        await program.InvokeAsync(args);
    }
}