using Aiursoft.CommandFramework.Extensions;
using Aiursoft.DotDownload.PluginFramework.Services.PluginFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CommandLine;
using Aiursoft.DotDownload.Http;

namespace Aiursoft.DotDownload.Tests;

[TestClass]
public class IntegrationTests
{
    private readonly RootCommand _program;

    public IntegrationTests()
    {
        _program = new RootCommand("Test env.")
            .AddGlobalOptions()
            .AddPlugins(new HttpPlugin());
    }

    [TestMethod]
    public async Task InvokeHelp()
    {
        var result = await _program.InvokeAsync(new[] { "--help" });
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public async Task InvokeVersion()
    {
        var result = await _program.InvokeAsync(new[] { "--version" });
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public async Task InvokeUnknown()
    {
        var result = await _program.InvokeAsync(new[] { "--wtf" });
        Assert.AreEqual(1, result);
    }

    [TestMethod]
    public async Task InvokeWithoutArg()
    {
        var result = await _program.InvokeAsync(Array.Empty<string>());
        Assert.AreEqual(1, result);
    }
    
    [TestMethod]
    public async Task InvokeDownload()
    {
        // Prepare
        var tempFolder = Path.Combine(Path.GetTempPath(), $"DotDownload-UT-{Guid.NewGuid()}");
        var tempFile = Path.Combine(tempFolder, "testfile.bin");
        if (!Directory.Exists(tempFolder))
        {
            Directory.CreateDirectory(tempFolder);
        }

        // Run
        var result = await _program.InvokeAsync(new[]
        {
            "download",
            "--url",
            "https://sgp-ping.vultr.com/vultr.com.100MB.bin",
            "--file",
            tempFile,
            "--verbose",
            "--threads",
            "16"
        });

        // Assert
        Assert.AreEqual(0, result);
        Assert.IsTrue(File.Exists(tempFile));
        Assert.AreEqual(new FileInfo(tempFile).Length, 100 * 1024 * 1024);
    }
}

