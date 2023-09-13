using Aiursoft.Scanner.Abstractions;

namespace Aiursoft.DotDownload.PluginFramework.Handlers.Download;

public class DiskService : ITransientDependency
{
    public void CreateFileAndAllocateSpace(string path, long length)
    {
        using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
        fs.SetLength(length);
        fs.Close();
    }
    
    public async Task SaveBlockToDisk(Stream stream, string path, long offset)
    {
        await using var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None,
            bufferSize: 4096, useAsync: true);
        fileStream.Seek(offset, SeekOrigin.Begin);
        await stream.CopyToAsync(fileStream);
        fileStream.Close();
    }
}