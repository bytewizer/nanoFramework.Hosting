using nanoFramework.Hosting.Http;
using System;
using System.Net.Http;

namespace nanoFramework.TestHarness
{
    internal class TypedClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TypedClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public string GetJson()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://httpbun.org/");

            HttpResponseMessage response = client.Get("get");
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsString();
        }
    }
}
