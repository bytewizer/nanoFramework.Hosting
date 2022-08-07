using System.Net.Http;

namespace nanoFramework.TestHarness
{
    internal class NamedClient
    {
        private readonly HttpClient _httpClient;

        public NamedClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public string GetJson()
        {
            HttpResponseMessage response = _httpClient.Get("get");
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsString();
        }
    }
}
