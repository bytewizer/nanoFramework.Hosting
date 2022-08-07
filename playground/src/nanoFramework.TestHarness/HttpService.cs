using System;
using System.Net.Http;

namespace nanoFramework.TestHarness
{
    internal class HttpService
    {
        private readonly HttpbunClient _httpbunClient;

        public HttpService(HttpbunClient client)
        {
            _httpbunClient = client;
        }

        public string GetJson()
        {
            HttpResponseMessage response = _httpbunClient.Client.Get("get");
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsString();
        }
    }
}
