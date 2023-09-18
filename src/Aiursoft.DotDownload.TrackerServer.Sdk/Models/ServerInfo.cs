using Aiursoft.AiurProtocol;
using Aiursoft.AiurVersionControl.CRUD;
using Aiursoft.DotDownload.Core.Models;
using System.Collections.Concurrent;

namespace Aiursoft.DotDownload.TrackerServer.Sdk.Models;

public class ServerInfo : AiurResponse
{
    public required string RequesterIp { get; set; }
    public required ConcurrentDictionary<string, CollectionRepository<HasRecord>> Total { get; set; }
}
