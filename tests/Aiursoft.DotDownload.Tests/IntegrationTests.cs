using Aiursoft.CommandFramework.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.CommandLine;
using Aiursoft.DotDownload.Http;
using Aiursoft.DotDownload.PluginFramework;

namespace Aiursoft.DotDownload.Tests;

[TestClass]
public class IntegrationTests
{
    private readonly RootCommand _program = new RootCommand("Test env.")
        .AddGlobalOptions()
        .AddPlugins(new HttpPlugin());

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
            "https://videos.aiursoft.cn/media/encoded/13/anduin/eeca2cf5a82b4a12a399b90a1a7a7dfa.eeca2cf5a82b4a12a399b90a1a7a7dfa.2023-03-05_13-20-08.mkv.mp4",
            "--file",
            tempFile,
            "--verbose",
            "--threads",
            "16"
        });

        // Assert
        Assert.AreEqual(0, result);
        Assert.IsTrue(File.Exists(tempFile));
        Assert.AreEqual(70013908, new FileInfo(tempFile).Length);
    }
}

