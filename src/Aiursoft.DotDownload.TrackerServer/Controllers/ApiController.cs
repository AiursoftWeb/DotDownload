using Aiursoft.AiurProtocol;
using Aiursoft.AiurProtocol.Server;
using Aiursoft.DotDownload.Core.Services;
using Aiursoft.Download.TrackerServer.Sdk.Models;
using Microsoft.AspNetCore.Mvc;
using Aiursoft.AiurEventSyncer.WebExtends;

namespace Aiursoft.Download.TrackerServer.Controllers;

[Route("api")]
public class ApiController : Controller
{
    private readonly InMemoryRepositoryManager repos;

    public ApiController(InMemoryRepositoryManager repos)
    {
        this.repos = repos;
    }

    [Route("info")]
    public IActionResult Info()
    {
        return this.Protocol(new ServerInfo
        {
            Code = Code.ResultShown,
            Message = "This is a Aiursoft.DotDownload Tracker server.",
            RequesterIp = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? throw new InvalidOperationException("IP is null!"),

            // Debug usage.
            Total = repos.GetTotal()
        });
    }

    [Route("{channel}/repo.ares")]
    public Task<IActionResult> ReturnRepoDemo([FromRoute]string channel, [FromQuery]string start)
    {
        var repo = repos.GetCollection(channel);
        return HttpContext.RepositoryAsync(repo, start);
    }
}
