using MauiAppBlockchain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
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

        public async Task<BlocksData> GetBlocks()
        {
            return await _httpClient.GetFromJsonAsync<BlocksData>("api/Chain");
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {         
            return await _httpClient.GetFromJsonAsync<IEnumerable<Category>>("api/Category");
        }
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<User>>("api/User");
        }

        public async Task<HttpResponseMessage> CreateBlock(HttpContent content)
        {
            return await _httpClient.PostAsync("api/Chain", content);
        }

        public async Task<HttpResponseMessage> CreateCategory(HttpContent content)
        {
            return await _httpClient.PostAsync("api/Category", content);
        }
        public async Task<HttpResponseMessage> CreateUser(HttpContent content)
        {
            return await _httpClient.PostAsync("api/User", content);
        }

        public async Task<HttpResponseMessage> Authentication(HttpContent content)
        {
            return await _httpClient.PostAsync("api/User/login", content);
        }

        public async Task<string> GetIpHost()
        {
            string Host = System.Net.Dns.GetHostName();
            string IP = System.Net.Dns.GetHostByName(Host).AddressList[0].ToString();
            return Host + "  " + IP;
        }
    }
}
