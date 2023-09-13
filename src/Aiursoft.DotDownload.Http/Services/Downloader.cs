using System.Net.Http.Headers;
using Aiursoft.Canon;
using Microsoft.Extensions.Logging;

namespace Aiursoft.DotDownload.PluginFramework.Handlers.Download;

public class Downloader
{
    private readonly HttpClient _httpClient;
    private readonly CanonPool _downloadPool;
    private readonly CanonQueue _writePool;
    private readonly ILogger<Downloader> _logger;

    public Downloader(
        HttpClient httpClient,
        CanonPool downloadPool,
        CanonQueue writePool,
        ILogger<Downloader> logger)
    {
        _httpClient = httpClient;
        _downloadPool = downloadPool;
        _writePool = writePool;
        _logger = logger;
    }

    public async Task DownloadAsync(
        string url,
        string savePath,
        long blockSize = 4 * 1024 * 1024,
        int threads = 16)
    {
        if (string.IsNullOrWhiteSpace(savePath))
        {
            // Auto generate based on url:
            // savePath = ...
        }

        _logger.LogTrace($"Requesting {url}...");
        var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        var contentLength = response.Content.Headers.ContentLength ?? 0;
        _logger.LogInformation("File length: {ContentLength}MB", contentLength/1024/1024);
        
        // TODO: What if server 301 or 302?
        // TODO: If the file doesn't support multiple threads downloading?

        // Create the file with the specified length.
        File.Delete(savePath);
        await using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None,
                         bufferSize: 4096, useAsync: true))
        {
            fileStream.SetLength(contentLength);
            fileStream.Close();
        }

        long blockCount = (long)Math.Ceiling((double)contentLength / blockSize);
        _logger.LogInformation("Blocks count: {BlockCount}", blockCount);

        for (var i = 0; i < blockCount; i++)
        {
            long offset = i * blockSize;
            var length = Math.Min(blockSize, contentLength - offset);
            _downloadPool.RegisterNewTaskToPool(async () =>
            {
                _logger.LogTrace(
                    $"Starting download with offset: {offset / 1024 / 1024}MB, length {length / 1024 / 1024}MB, totally {contentLength / 1024 / 1024}MB");
                var fileStream = await DownloadBlockAsync(url, offset, length);
                //var bytes = UseStreamDotReadMethod(fileStream);
                _logger.LogDebug($"Downloaded block from offset: {offset / 1024 / 1024}MB, stream length is {fileStream.Length}");
                _writePool.QueueNew(async () =>
                    {
                        _logger.LogTrace($"Saving block {offset / 1024 / 1024}MB to {(offset + length) / 1024 / 1024}MB.");
                        await SaveBlockToDisk(fileStream, savePath, offset);
                        _logger.LogTrace($"Finish block {offset / 1024 / 1024}MB to {(offset + length) / 1024 / 1024}MB.");
                    },
                    startTheEngine: false);
                _logger.LogTrace(
                    $"Finished download with offset: {offset / 1024 / 1024}MB, length {length / 1024 / 1024}MB, totally {contentLength / 1024 / 1024}MB");
            });
        }

        await _downloadPool.RunAllTasksInPoolAsync(threads);
        await _writePool.RunTasksInQueue(1);
    }

    private async Task<MemoryStream> DownloadBlockAsync(string url, long offset, long length)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Range = new RangeHeaderValue(offset, offset + length - 1);
        var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();
        var remoteStream = await response.Content.ReadAsStreamAsync();
        var memoryStream = new MemoryStream();
        await remoteStream.CopyToAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }

    private async Task SaveBlockToDisk(MemoryStream stream, string path, long offset)
    {
        await using var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None,
            bufferSize: 4096, useAsync: true);
        fileStream.Seek(offset, SeekOrigin.Begin);
        await stream.CopyToAsync(fileStream);
        fileStream.Close();
    }
}