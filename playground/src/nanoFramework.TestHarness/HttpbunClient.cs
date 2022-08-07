using nanoFramework.Hosting.Http;
using System;
using System.Net.Http;

namespace nanoFramework.TestHarness
{
    internal class HttpbunClient
    {
        public HttpClient Client { get; private set; }

        public HttpbunClient(HttpClient httpClient)
        {
            Client = httpClient;
        }

        public string GetJson()
        {
            HttpResponseMessage response = Client.Get("get");
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsString();
        }
    }
}
