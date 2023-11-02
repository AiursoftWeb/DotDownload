using Aiursoft.DotDownload.PluginFramework;
using Aiursoft.CommandFramework;
using Aiursoft.CommandFramework.Extensions;
using Aiursoft.DotDownload.Http;

return await new AiursoftCommand()
    .Configure(command =>
    {
        command
            .AddGlobalOptions()
            .AddPlugins(
                new HttpPlugin()
            );
    })
    .RunAsync(args);