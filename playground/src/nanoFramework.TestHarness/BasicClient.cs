using nanoFramework.Hosting.Http;
using System;
using System.Net.Http;

namespace nanoFramework.TestHarness
{
    internal class BasicClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;

        public BasicClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            _httpClient = _httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://httpbun.org/");
        }

        public string GetJson()
        {
            HttpResponseMessage response = _httpClient.Get("get");
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsString();
        }
    }
}
