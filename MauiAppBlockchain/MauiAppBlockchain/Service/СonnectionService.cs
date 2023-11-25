﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppBlockchain.Service
{
    public class СonnectionService
    {
        private readonly HttpClient _httpClient;
        public СonnectionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetBlocks()
        {
            return await _httpClient.GetStringAsync("api/Chain");
        }

        public async Task<HttpResponseMessage> CreateBlock(HttpContent content)
        {
            return await _httpClient.PostAsync("api/Chain", content);
        }
    }
}
