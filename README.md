# Aiursoft DotDownload

[![MIT licensed](https://img.shields.io/badge/license-MIT-blue.svg)](https://gitlab.aiursoft.cn/anduin/DotDownload/-/blob/master/LICENSE)
[![Pipeline stat](https://gitlab.aiursoft.cn/Aiursoft/DotDownload/badges/master/pipeline.svg)](https://gitlab.aiursoft.cn/Aiursoft/DotDownload/-/pipelines)
[![Test Coverage](https://gitlab.aiursoft.cn/Aiursoft/DotDownload/badges/master/coverage.svg)](https://gitlab.aiursoft.cn/Aiursoft/DotDownload/-/pipelines)
[![NuGet version (Aiursoft.DotDownload)](https://img.shields.io/nuget/v/Aiursoft.DotDownload.svg)](https://www.nuget.org/packages/Aiursoft.DotDownload/)
[![ManHours](https://manhours.aiursoft.cn/gitlab/gitlab.aiursoft.cn/aiursoft/dotdownload)](https://gitlab.aiursoft.cn/aiursoft/dotdownload/-/commits/master?ref_type=heads)

This project helps you download a file with multiple threads.

## Install

Requirements:

1. [.NET 7 SDK](http://dot.net/)

Run the following command to install this tool:

```bash
dotnet tool install --global Aiursoft.DotDownload
```

## Usage

After getting the binary, run it directly in the terminal.

```bash
$ dot-download download -u https://anduins-site.aiur.site/winner.mp4
```

That's it! The file will be downloaded to the current directory.
