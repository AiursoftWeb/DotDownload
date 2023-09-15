using Aiursoft.AiurVersionControl.CRUD;
using Aiursoft.DotDownload.Core.Models;
using Aiursoft.Scanner.Abstractions;
using System.Collections.Concurrent;

namespace Aiursoft.DotDownload.Core.Services;

public class InMemoryRepositoryManager : ISingletonDependency
{
    private ConcurrentDictionary<string, CollectionRepository<HasRecord>> InMemoryRepositories { get; set; } = new ConcurrentDictionary<string, CollectionRepository<HasRecord>>();
    private object reposLock = new object();

    public CollectionRepository<HasRecord> GetCollection(string channel)
    {
        lock (reposLock)
        {
            if (!InMemoryRepositories.ContainsKey(channel))
            {
                var collection = new CollectionRepository<HasRecord>();
                InMemoryRepositories[channel] = collection;
            }
        }
        return InMemoryRepositories[channel];
    }

    // Debug usage.
    public ConcurrentDictionary<string, CollectionRepository<HasRecord>> GetTotal()
    {
        return InMemoryRepositories;
    }
}
