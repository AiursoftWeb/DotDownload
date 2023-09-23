namespace Aiursoft.DotDownload.Core.Models;

public class HasRecord
{
    public required string IpAddress { get; set; }

    public required int BlockIndex { get; set; }

    public required DateTime RegisterTime { get; set; }
}
