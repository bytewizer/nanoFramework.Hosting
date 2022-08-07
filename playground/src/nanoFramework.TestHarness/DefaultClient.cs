using nanoFramework.Hosting.Http;
using System;
using System.Net.Http;

namespace nanoFramework.TestHarness
{
    internal class DefaultClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;

        public DefaultClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            _httpClient = _httpClientFactory.CreateClient();
        }

        public string GetJson()
        {
            HttpResponseMessage response = _httpClient.Get("get");
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsString();
        }
    }
}
