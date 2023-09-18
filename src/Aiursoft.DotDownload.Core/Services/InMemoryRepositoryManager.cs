using Aiursoft.AiurEventSyncer.Models;
using Aiursoft.DotDownload.Core.Models;
using Aiursoft.Scanner.Abstractions;
using System.Collections.Concurrent;

namespace Aiursoft.DotDownload.Core.Services;

public class InMemoryRepositoryManager : ISingletonDependency
{
    private ConcurrentDictionary<string, Repository<HasRecord>> InMemoryRepositories { get; set; } = new ConcurrentDictionary<string, Repository<HasRecord>>();
    private object reposLock = new object();

    public Repository<HasRecord> GetCollection(string channel)
    {
        lock (reposLock)
        {
            if (!InMemoryRepositories.ContainsKey(channel))
            {
                var collection = new Repository<HasRecord>();
                InMemoryRepositories[channel] = collection;
            }
        }
        return InMemoryRepositories[channel];
    }

    // Debug usage.
    public ConcurrentDictionary<string, Repository<HasRecord>> GetTotal()
    {
        return InMemoryRepositories;
    }
}
