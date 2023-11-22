using System;
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

        public Task<string> GetBlocks()
        {
            return _httpClient.GetStringAsync(new Uri("api/Chain"));
        }

        public Task<HttpResponseMessage> CreateBlock(HttpContent content)
        {
            return _httpClient.PostAsync("api/Chain", content);
        }
    }
}
