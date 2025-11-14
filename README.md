# Aiursoft DotDownload

[![MIT licensed](https://img.shields.io/badge/license-MIT-blue.svg)](https://gitlab.aiursoft.com/anduin/DotDownload/-/blob/master/LICENSE)
[![Pipeline stat](https://gitlab.aiursoft.com/Aiursoft/DotDownload/badges/master/pipeline.svg)](https://gitlab.aiursoft.com/Aiursoft/DotDownload/-/pipelines)
[![Test Coverage](https://gitlab.aiursoft.com/Aiursoft/DotDownload/badges/master/coverage.svg)](https://gitlab.aiursoft.com/Aiursoft/DotDownload/-/pipelines)
[![NuGet version (Aiursoft.DotDownload)](https://img.shields.io/nuget/v/Aiursoft.DotDownload.svg)](https://www.nuget.org/packages/Aiursoft.DotDownload/)
[![ManHours](https://manhours.aiursoft.com/r/gitlab.aiursoft.com/aiursoft/dotdownload.svg)](https://gitlab.aiursoft.com/aiursoft/dotdownload/-/commits/master?ref_type=heads)

This project helps you download a file with multiple threads.

## Installation

Requirements:

1. [.NET 10 SDK](http://dot.net/)

Run the following command to install this tool:

```bash
dotnet tool install --global Aiursoft.DotDownload
```

## Usage

After getting the binary, run it directly in the terminal.

```bash
$ dot-download https://anduins-site.aiur.site/winner.mp4
```

That's it! The file will be downloaded to the current directory.

## Run locally

Requirements about how to run

1. [.NET 10 SDK](http://dot.net/)
2. Execute `dotnet run` to run the app

## Run in Microsoft Visual Studio

1. Open the `.sln` file in the project path.
2. Press `F5`.

## How to contribute

There are many ways to contribute to the project: logging bugs, submitting pull requests, reporting issues, and creating suggestions.

Even if you with push rights on the repository, you should create a personal fork and create feature branches there when you need them. This keeps the main repository clean and your workflow cruft out of sight.

We're also interested in your feedback on the future of this project. You can submit a suggestion or feature request through the issue tracker. To make this process more effective, we're asking that these include more information to help define them more clearly.
