using System.Diagnostics;
using Aiursoft.Canon;
using Aiursoft.DotDownload.Core.Models;
using Aiursoft.Scanner.Abstractions;
using Microsoft.Extensions.Logging;

namespace Aiursoft.DotDownload.Core.Services;

public class Downloader : ITransientDependency
{
    private readonly DiskService _diskService;
    private readonly HttpClient _httpClient;
    private readonly CanonPool _downloadPool;
    private readonly CanonQueue _diskWriteQueue;
    private readonly HttpBlockDownloader httpBlockDownloader;
    private readonly ILogger<Downloader> _logger;

    public Downloader(
        DiskService diskService,
        HttpClient httpClient,
        CanonPool downloadPool,
        CanonQueue writePool,
        HttpBlockDownloader httpBlockDownloader,
        ILogger<Downloader> logger)
    {
        _diskService = diskService;
        _httpClient = httpClient;
        _downloadPool = downloadPool;
        _diskWriteQueue = writePool;
        this.httpBlockDownloader = httpBlockDownloader;
        _logger = logger;
    }

    /// <summary>
    /// This is a C# method that downloads a file asynchronously from a given URL and saves it to a specified location on the local disk.
    /// </summary>
    /// <param name="url">Http URL to download.</param>
    /// <param name="savePath">Path to save.</param>
    /// <param name="blockSize">Block size. It will download every blocks in parallel and save those blocks to disk with a queue.</param>
    /// <param name="threads">Max threads to download.</param>
    /// <param name="showProgressBar">If to show the progress bar.</param>
    public async Task DownloadAsync(
        string url,
        string savePath,
        long blockSize = 4 * 1024 * 1024,
        int threads = 16,
        bool showProgressBar = false)
    {
        ProgressBar? bar = null;
        var watch = Stopwatch.StartNew();
        watch.Start();
        
        _logger.LogTrace($"Requesting {url}...");
        var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        var fileLength = response.Content.Headers.ContentLength ?? 0;
        _logger.LogInformation("File length: {ContentLength}MB", fileLength / 1024 / 1024);

        // TODO: What if server 301 or 302? Follow redirect.
        if (response.Headers.AcceptRanges.Contains("bytes") != true)
        {
            _logger.LogWarning("The server doesn't support multiple threads downloading. Using single block...");
            showProgressBar = false;
            threads = 1;
            blockSize = fileLength;
        }

        savePath = PathExtensions.GetFilePathToSave(savePath, url);
        _logger.LogInformation($"File will be saved to {savePath}...");

        if (File.Exists(savePath))
        {
            _logger.LogWarning("File already exists. Deleting...");
            File.Delete(savePath);
        }
        
        _diskService.CreateFileAndAllocateSpace(savePath, fileLength);
        var blockCount = (long)Math.Ceiling((double)fileLength / blockSize);
        _logger.LogInformation("Blocks count: {BlockCount}", blockCount);

        if (showProgressBar) bar = new ProgressBar();
        var savedBlocks = 0;
        for (var i = 0; i < blockCount; i++)
        {
            var offset = i * blockSize;
            var length = Math.Min(blockSize, fileLength - offset);
            _downloadPool.RegisterNewTaskToPool(async () =>
            {
                _logger.LogTrace(
                    $"Starting download with offset: {offset / 1024 / 1024}MB, length {length / 1024 / 1024}MB, totally {fileLength / 1024 / 1024}MB");
                var block = await httpBlockDownloader.DownloadBlockAsync(url, offset, length);
                _diskWriteQueue.QueueNew(async () =>
                    {
                        _logger.LogTrace(
                            $"Saving block {offset / 1024 / 1024}MB to {(offset + length) / 1024 / 1024}MB on local disk...");
                        await _diskService.SaveBlockToDisk(block, savePath, offset);
                        savedBlocks++;
                        bar?.Report((double)savedBlocks / blockCount);
                        _logger.LogTrace(
                            $"Finish block {offset / 1024 / 1024}MB to {(offset + length) / 1024 / 1024}MB to save on disk.");
                    },
                    startTheEngine: true,
                    maxThreads: 1); // Only one thread to write to disk.
                _logger.LogTrace(
                    $"Finished download with offset: {offset / 1024 / 1024}MB, length {length / 1024 / 1024}MB, totally {fileLength / 1024 / 1024}MB");
            });
        }

        await _downloadPool.RunAllTasksInPoolAsync(threads);
        await _diskWriteQueue.Engine; // Make sure the write pool is finished.
        bar?.Dispose();
        
        watch.Stop();
        _logger.LogTrace("Download finished in {Elapsed} seconds.", watch.Elapsed.TotalSeconds);
        _logger.LogInformation("Download speed: {Speed}MB/s", (double)fileLength / 1024 / 1024 / watch.Elapsed.TotalSeconds);
    }
}