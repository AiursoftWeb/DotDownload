using Aiursoft.AiurEventSyncer.Models;
using Aiursoft.AiurProtocol;
using Aiursoft.DotDownload.Core.Models;
using System.Collections.Concurrent;

namespace Aiursoft.DotDownload.TrackerServer.Sdk.Models;

public class ServerInfo : AiurResponse
{
    public required string RequesterIp { get; set; }
    public required ConcurrentDictionary<string, Repository<HasRecord>> Total { get; set; }
}
