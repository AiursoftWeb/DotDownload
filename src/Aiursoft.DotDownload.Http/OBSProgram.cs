using Aiursoft.Canon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace Downloader;

public static class Program
{
    public static async Task Main(string[] args)
    {
        await DownloadAsync(
            "https://software.download.prss.microsoft.com/dbazure/Win8.1_SingleLang_English_x64.iso?t=3ff91fa5-4902-4e8b-a57e-fc334b3152cd&e=1694684523&h=fb4bdca92a00c04ca3a8a163d544e772bb47ae4140e8c6d0e6dff8dc3b4475db",
            "D:\\a.iso");
    }

    public static async Task DownloadAsync(
        string url,
        string savePath,
        long blockSize = 4 * 1024 * 1024,
        int threads = 16,
        bool verbose = false)
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        var contentLength = response.Content.Headers.ContentLength ?? 0;

        // Create the file with the specified length.
        File.Delete(savePath);
        using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
        {
            fileStream.SetLength(contentLength);
            fileStream.Close();
        }

        long blockCount = (long)Math.Ceiling((double)contentLength / blockSize);

        var hostBuilder = new HostBuilder();
        hostBuilder.ConfigureServices(services =>
        {
            services.AddTaskCanon();
        });
        hostBuilder.ConfigureLogging(logging =>
        {
            logging
                .AddFilter("Microsoft.Extensions", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning);
            logging.AddSimpleConsole(options =>
            {
                options.IncludeScopes = verbose;
                options.SingleLine = true;
                options.TimestampFormat = "mm:ss ";
                options.UseUtcTimestamp = true;
            });

            logging.SetMinimumLevel(verbose ? LogLevel.Trace : LogLevel.Information);
        });
        var host = hostBuilder.Build();
        await host.StartAsync();

        var downloadPool = host.Services.GetRequiredService<CanonPool>();
        var writePool = host.Services.GetRequiredService<CanonQueue>();
        for (var i = 0; i < blockCount; i++)
        {
            long offset = i * blockSize;
            var length = Math.Min(blockSize, contentLength - offset);
            downloadPool.RegisterNewTaskToPool(async () =>
            {
                Console.WriteLine($"Starting download with offset: {offset / 1024 / 1024}MB, length {length / 1024 / 1024}MB");
                var fileStream = await DownloadBlockAsync(httpClient, url, offset, length);
                //var bytes = UseStreamDotReadMethod(fileStream);
                Console.WriteLine($"Downloaded stream length is {fileStream.Length}");
                writePool.QueueNew(async () =>
                {
                    await SaveBlockToDisk(fileStream, savePath, offset, length);
                }, startTheEngine: false);
                Console.WriteLine($"Finished download with offset: {offset / 1024 / 1024}MB, length {length / 1024 / 1024}MB");
            });
        }

        await downloadPool.RunAllTasksInPoolAsync(threads);
        await writePool.RunTasksInQueue(1);
    }

    private static async Task<MemoryStream> DownloadBlockAsync(HttpClient httpClient, string url, long offset, long length)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Range = new RangeHeaderValue(offset, offset + length - 1);
        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();
        var remoteStream = await response.Content.ReadAsStreamAsync();
        var memoryStream = new MemoryStream();
        await remoteStream.CopyToAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }

    private static async Task SaveBlockToDisk(MemoryStream stream, string path, long offset, long length)
    {
        using (var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, bufferSize: 4096, useAsync: true))
        {
            Console.WriteLine($"Writing block {offset / 1024 / 1024}MB to {(offset + length) / 1024 / 1024}MB.");
            fileStream.Seek(offset, SeekOrigin.Begin);
            await stream.CopyToAsync(fileStream);
            Console.WriteLine($"Finishs block {offset / 1024 / 1024}MB to {(offset + length) / 1024 / 1024}MB.");
            fileStream.Close();
        }
    }
}