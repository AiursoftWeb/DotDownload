# Aiursoft DotDownload

[![MIT licensed](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/AiursoftWeb/dotdownload/blob/master/LICENSE)
[![Pipeline stat](https://gitlab.aiursoft.com/Aiursoft/DotDownload/badges/master/pipeline.svg)](https://gitlab.aiursoft.com/Aiursoft/DotDownload/-/pipelines)
[![Test Coverage](https://gitlab.aiursoft.com/Aiursoft/DotDownload/badges/master/coverage.svg)](https://gitlab.aiursoft.com/Aiursoft/DotDownload/-/pipelines)
[![S360 compliance](https://s360.aiursoft.com/badge.svg?project=aiursoft%2Fdotdownload)](https://s360.aiursoft.com/SupplyChain?tab=projects&project=aiursoft%2Fdotdownload&scope=baseline&ecosystem=all)
[![NuGet version (Aiursoft.DotDownload)](https://img.shields.io/nuget/v/Aiursoft.DotDownload.svg)](https://www.nuget.org/packages/Aiursoft.DotDownload/)
[![Man hours](https://manhours.aiursoft.com/r/github.com/aiursoftweb/dotdownload.svg)](https://manhours.aiursoft.com/r/github.com/aiursoftweb/dotdownload.html)

This project helps you download a file with multiple threads.

## Overview

### What is DotDownload?

Download files over HTTP using parallel connections, with configurable output path, connection count and block size.

## Installation

Install the CLI with the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0):

```bash
dotnet tool install --global Aiursoft.DotDownload
```

Update an existing installation:

```bash
dotnet tool update --global Aiursoft.DotDownload
```

Remove the CLI:

```bash
dotnet tool uninstall --global Aiursoft.DotDownload
```

## Usage

Run the command to see available options:

```bash
dot-download --help
```

After getting the binary, run it directly in the terminal:

```bash
dot-download https://download.anduinos.com/1.4/1.4.0/AnduinOS-1.4.0-en_US.iso
```

The file is downloaded to the current directory unless an output path is supplied.

### Examples and output

Choose an output file and limit the number of parallel connections:

```bash
dot-download --url https://example.com/archive.zip --file ./archive.zip --threads 4
```

Normal output includes a progress bar when the server supports ranged downloads. Use `--verbose` for detailed framework logging instead. Download sizes, speed and duration depend on the server and network. Servers without byte-range support use a single download block. An existing output file is deleted before the replacement download is written.

## Configuration

Configuration is supplied through command-line options. The URL can also be passed positionally. No additional configuration file is required.

**Command-line options**

| Option | Meaning | Default |
| --- | --- | --- |
| `-u`, `--url` | HTTP URL to download; may also be positional | Required |
| `-f`, `--file` | Output file path | Empty; derive the file name and save in the current directory |
| `-t`, `--threads` | Maximum parallel download connections | `16` |
| `-b`, `--block-size` | Download block size in bytes | `4194304` (4 MiB) |
| `-v`, `--verbose` | Detailed framework logging instead of the progress bar | `false` |
| `--version` | Show version information | — |
| `-?`, `-h`, `--help` | Show current help | — |

## Development

Install the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).

From the repository root, restore and build:

```bash
dotnet restore Aiursoft.DotDownload.sln
dotnet build Aiursoft.DotDownload.sln --no-restore
```

<a name="run-locally"></a>
Run the tool from source:

```bash
dotnet run --project src/Aiursoft.DotDownload/Aiursoft.DotDownload.csproj -- --help
```

<a name="run-in-microsoft-visual-studio"></a>
In Microsoft Visual Studio, open `Aiursoft.DotDownload.sln`, select `Aiursoft.DotDownload` as the startup project and press `F5`. Set the command-line arguments for the task you want to run.

Run the existing tests:

```bash
dotnet test Aiursoft.DotDownload.sln --no-restore
```

## How to contribute

There are many ways to contribute to the project: logging bugs, submitting pull requests, reporting issues, and creating suggestions.

Even if you have push rights on the repository, you should create a personal fork and create feature branches there when you need them. This keeps the main repository clean and your workflow cruft out of sight.

We're also interested in your feedback on the future of this project. You can submit a suggestion or feature request through the issue tracker. To make this process more effective, we're asking that these include more information to help define them more clearly.

## License

Licensed under the [MIT License](https://github.com/AiursoftWeb/dotdownload/blob/master/LICENSE).
