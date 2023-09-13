using System.Net.Http.Headers;
using Aiursoft.Canon;
using Aiursoft.DotDownload.Http.Models;
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
        var fileToWrite = savePath;
        if (string.IsNullOrWhiteSpace(fileToWrite))
        {
            fileToWrite = "./" + Path.GetFileName(url);
        }

        fileToWrite = GetAbsolutePath(Directory.GetCurrentDirectory(), fileToWrite);

        _logger.LogTrace($"Requesting {url}...");
        var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        var contentLength = response.Content.Headers.ContentLength ?? 0;
        _logger.LogInformation("File length: {ContentLength}MB", contentLength / 1024 / 1024);

        // TODO: What if server 301 or 302?
        // TODO: If the file doesn't support multiple threads downloading?

        // Create the file with the specified length.
        _logger.LogInformation($"File will be saved to {fileToWrite}...");
        File.Delete(fileToWrite);
        await using (var fileStream = new FileStream(fileToWrite, FileMode.Create, FileAccess.Write, FileShare.None,
                         bufferSize: 4096, useAsync: true))
        {
            fileStream.SetLength(contentLength);
            fileStream.Close();
        }

        var blockCount = (long)Math.Ceiling((double)contentLength / blockSize);
        _logger.LogInformation("Blocks count: {BlockCount}", blockCount);

        var bar = new ProgressBar();
        var savedBlocks = 0;
        for (var i = 0; i < blockCount; i++)
        {
            var offset = i * blockSize;
            var length = Math.Min(blockSize, contentLength - offset);
            _downloadPool.RegisterNewTaskToPool(async () =>
            {
                _logger.LogTrace(
                    $"Starting download with offset: {offset / 1024 / 1024}MB, length {length / 1024 / 1024}MB, totally {contentLength / 1024 / 1024}MB");
                var fileStream = await DownloadBlockAsync(url, offset, length);
                _writePool.QueueNew(async () =>
                    {
                        _logger.LogTrace(
                            $"Saving block {offset / 1024 / 1024}MB to {(offset + length) / 1024 / 1024}MB on local disk...");
                        await SaveBlockToDisk(fileStream, fileToWrite, offset);
                        savedBlocks++;
                        bar.Report((double)savedBlocks / blockCount);
                        _logger.LogTrace(
                            $"Finish block {offset / 1024 / 1024}MB to {(offset + length) / 1024 / 1024}MB to save on disk.");
                    },
                    startTheEngine: true,
                    maxThreads: 1);
                _logger.LogTrace(
                    $"Finished download with offset: {offset / 1024 / 1024}MB, length {length / 1024 / 1024}MB, totally {contentLength / 1024 / 1024}MB");
            });
        }

        await _downloadPool.RunAllTasksInPoolAsync(threads);
        await _writePool.Engine;
        bar.Dispose();
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

    private static string GetAbsolutePath(string currentPath, string referencePath)
    {
        if (Path.IsPathRooted(referencePath)) return referencePath;

        referencePath = referencePath.Replace('\\', Path.DirectorySeparatorChar);

        return Path.GetFullPath(Path.Combine(currentPath, referencePath));
    }
}