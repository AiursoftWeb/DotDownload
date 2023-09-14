﻿using Aiursoft.Scanner.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Aiursoft.DotDownload.Http.Services
{
    public class HttpBlockDownloader : ITransientDependency
    {
        private readonly HttpClient _httpClient;

        public HttpBlockDownloader(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<MemoryStream> DownloadBlockAsync(string url, long offset, long length)
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
    }
}
