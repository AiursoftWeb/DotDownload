using Aiursoft.Canon;
using Aiursoft.CommandFramework.ProgressBar;
using Aiursoft.Scanner.Abstractions;
using Microsoft.Extensions.Logging;

namespace Aiursoft.DotDownload.Core.Services;

public class Downloader(
    WatchService watchService,
    DiskService diskService,
    IHttpClientFactory httpClient,
    CanonPool downloadPool,
    CanonQueue writePool,
    HttpBlockDownloader httpBlockDownloader,
    ILogger<Downloader> logger)
    : ITransientDependency
{
    private readonly HttpClient _httpClient = httpClient.CreateClient(nameof(Downloader));

    /// <summary>
    /// This is a C# method that downloads a file asynchronously from a given URL and saves it to a specified location on the local disk.
    /// </summary>
    /// <param name="url">Http URL to download.</param>
    /// <param name="savePath">Path to save.</param>
    /// <param name="blockSize">Block size. It will download every block in parallel and save those blocks to disk with a queue.</param>
    /// <param name="threads">Max threads to download.</param>
    /// <param name="showProgressBar">If to show the progress bar.</param>
    public async Task DownloadWithWatchAsync(
        string url,
        string savePath,
        long blockSize = 4 * 1024 * 1024,
        int threads = 16,
        bool showProgressBar = false)
    {
        var (elapsed, fileLength) = await watchService.RunWithWatchAsync(() => DownloadAsync(url, savePath, blockSize, threads, showProgressBar));       
        logger.LogTrace("Download finished in {Elapsed} seconds.", elapsed.TotalSeconds);
        logger.LogInformation("Download speed: {Speed}MB/s", (double)fileLength / 1024 / 1024 / elapsed.TotalSeconds);
    }

    private async Task<long> DownloadAsync(
        string url,
        string savePath,
        long blockSize,
        int threads,
        bool showProgressBar)
    {
        ProgressBar? bar = null;
        logger.LogTrace($"Requesting {url}...");
        var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        var fileLength = response.Content.Headers.ContentLength ?? 0;
        logger.LogInformation("File length: {ContentLength}MB", fileLength / 1024 / 1024);

        if (!response.Headers.AcceptRanges.Contains("bytes"))
        {
            logger.LogWarning("The server doesn't support multiple threads downloading. Using single block...");
            showProgressBar = false;
            threads = 1;
            blockSize = fileLength;
        }

        savePath = PathExtensions.GetFilePathToSave(savePath, url);
        logger.LogInformation($"File will be saved to {savePath}...");

        if (File.Exists(savePath))
        {
            logger.LogWarning("File already exists. Deleting...");
            File.Delete(savePath);
        }

        diskService.CreateFileAndAllocateSpace(savePath, fileLength);
        var blockCount = (long)Math.Ceiling((double)fileLength / blockSize);
        logger.LogInformation("Blocks count: {BlockCount}", blockCount);

        if (showProgressBar) bar = new ProgressBar();
        var savedBlocks = 0;
        for (var i = 0; i < blockCount; i++)
        {
            var offset = i * blockSize;
            var length = Math.Min(blockSize, fileLength - offset);
            downloadPool.RegisterNewTaskToPool(async () =>
            {
                logger.LogTrace(
                    $"Starting download with offset: {offset / 1024 / 1024}MB, length {length / 1024 / 1024}MB, totally {fileLength / 1024 / 1024}MB");
                var block = await httpBlockDownloader.DownloadBlockAsync(url, offset, length);
                writePool.QueueNew(async () =>
                {
                    logger.LogTrace(
                        $"Saving block {offset / 1024 / 1024}MB to {(offset + length) / 1024 / 1024}MB on local disk...");
                    await diskService.SaveBlockToDisk(block, savePath, offset);
                    savedBlocks++;
                    
                    // ReSharper disable once AccessToDisposedClosure
                    bar?.Report((double)savedBlocks / blockCount);
                    logger.LogTrace(
                        $"Finish block {offset / 1024 / 1024}MB to {(offset + length) / 1024 / 1024}MB to save on disk.");
                },
                    startTheEngine: true,
                    maxThreads: 1); // Only one thread to write to disk.
                logger.LogTrace(
                    $"Finished download with offset: {offset / 1024 / 1024}MB, length {length / 1024 / 1024}MB, totally {fileLength / 1024 / 1024}MB");
            });
        }

        await downloadPool.RunAllTasksInPoolAsync(threads);
        await writePool.Engine; // Make sure the write pool is finished.
        bar?.Dispose();

        return fileLength;
    }
}