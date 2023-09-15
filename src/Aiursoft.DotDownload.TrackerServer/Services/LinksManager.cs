using Aiursoft.Canon;
using Aiursoft.Scanner.Abstractions;
using Aiursoft.Download.TrackerServer.Sdk.Models;
using Microsoft.Extensions.Options;

namespace Aiursoft.Download.TrackerServer.Services
{
    public class LinksManager : IScopedDependency
    {
        private readonly List<KnownLink> initialLinks;

        public LinksManager(
            CacheService cacheService,
            IOptions<List<KnownLink>> initialLinks) 
        {
            this.initialLinks = initialLinks.Value;
        }
    }
}
