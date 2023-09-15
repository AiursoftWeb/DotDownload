using Aiursoft.Scanner.Abstractions;
using Aiursoft.Download.TrackerServer.Sdk.Models;
using Aiursoft.Download.TrackerServer.Sdk.Services;
using Microsoft.Extensions.Options;

namespace Aiursoft.Download.TrackerServer.Services
{
    public class BootService : IScopedDependency
    {
        private readonly List<KnownLink> initialLinks;
        private readonly IConfiguration configuration;
        private readonly TrackerAccess trackerAccess;

        public BootService(
            IConfiguration configuration,
            IOptions<List<KnownLink>> initialLinks,
            TrackerAccess trackerAccess)
        {
            this.initialLinks = initialLinks.Value;
            this.configuration = configuration;
            this.trackerAccess = trackerAccess;
        }

        public async Task Boot()
        {
            foreach (var initLink in this.initialLinks)
            {
                await trackerAccess.RegisterOnRemoteAsync(initLink.ServerEndpoint!, configuration["Endpoint"]!);
            }
        }
    }
}
