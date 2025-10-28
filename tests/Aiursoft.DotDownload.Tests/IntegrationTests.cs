using Aiursoft.CommandFramework;
using Aiursoft.DotDownload.Http.Handlers.Download;
using Aiursoft.DotDownload.PluginFramework;

namespace Aiursoft.DotDownload.Tests;

[TestClass]
public class IntegrationTests
{
    private readonly SingleCommandApp<DownloadHandler> _program = new SingleCommandApp<DownloadHandler>()
        .WithDefaultOption(OptionsProvider.Url);

    [TestMethod]
    public async Task InvokeHelp()
    {
        var result = await _program.TestRunAsync(["--help"]);
        Assert.AreEqual(0, result.ProgramReturn);
    }

    [TestMethod]
    public async Task InvokeVersion()
    {
        var result = await _program.TestRunAsync(["--version"]);
        Assert.AreEqual(0, result.ProgramReturn);
    }

    [TestMethod]
    public async Task InvokeUnknown()
    {
        var result = await _program.TestRunAsync(["--wtf"]);
        Assert.AreEqual(1, result.ProgramReturn);
    }

    [TestMethod]
    public async Task InvokeWithoutArg()
    {
        var result = await _program.TestRunAsync([]);
        Assert.AreEqual(1, result.ProgramReturn);
    }

    [TestMethod]
    public async Task InvokeDownload()
    {
        // Prepare
        var tempFolder = Path.Combine(Path.GetTempPath(), $"DotDownload-UT-{Guid.NewGuid()}");
        var tempFile = Path.Combine(tempFolder, "test_file.bin");
        if (!Directory.Exists(tempFolder))
        {
            Directory.CreateDirectory(tempFolder);
        }

        // Run
        var result = await _program.TestRunAsync([
            "https://pub.aiursoft.com/nssm.exe",
            "--file",
            tempFile,
            "--verbose",
            "--threads",
            "16"
        ], OptionsProvider.Url);

        // Assert
        Assert.AreEqual(0, result.ProgramReturn);
        Assert.IsTrue(File.Exists(tempFile));
        Assert.AreEqual(331264, new FileInfo(tempFile).Length);

        // Clean
        File.Delete(tempFile);
    }
}

