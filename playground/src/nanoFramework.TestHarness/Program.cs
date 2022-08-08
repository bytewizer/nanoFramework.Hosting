using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;

using nanoFramework.Networking;
using nanoFramework.Hosting.Http;
using nanoFramework.DependencyInjection;
using nanoFramework.Hosting;
using nanoFramework.Hosting.Configuration;

namespace nanoFramework.TestHarness
{
    // nanoff --target M5Core2 --serialport COM9 --update

    public class Program
    {
        static readonly HttpClient _httpClient = new HttpClient();

        public static void Main()
        {
            SetupWifi();

            var builder = Host.CreateDefaultBuilder()

                .ConfigureServices((context, services) =>
                {
                    services.AddHttpClient(NamedClients.Httpbun, client =>
                    {
                        client.BaseAddress = new Uri("http://httpbun.org/");
                    });

                    services.AddHttpClient(client =>
                    {
                        client.BaseAddress = new Uri("http://httpbun.org/");
                    });

                    services.AddSingleton(typeof(DefaultClient));
                    services.AddSingleton(typeof(BasicClient));
                    services.AddSingleton(typeof(NamedClient));
                });

            var host = builder.Build();

            var defaultClient = (DefaultClient)host.Services.GetService(typeof(DefaultClient));
            Debug.WriteLine(defaultClient.GetJson());

            var basicClient = (BasicClient)host.Services.GetService(typeof(BasicClient));
            Debug.WriteLine(basicClient.GetJson());

            var namedClient = (NamedClient)host.Services.GetService(typeof(NamedClient));
            Debug.WriteLine(namedClient.GetJson());
        }

        private static void SetupWifi()
        {
            var success = WifiNetworkHelper.ConnectDhcp("ssid", "!password!");

            if (success)
            {
                Debug.WriteLine("IP: " + NetworkInterface.GetAllNetworkInterfaces()[0].IPv4Address);
            }
            else
            {
                Debug.WriteLine($"Failed to establish IP address and DateTime, error: {NetworkHelper.HelperException}.");
            }
        }

        private static void GetJsonResponse()
        {
            var response = _httpClient.Get("http://httpbun.org/get");
            response.EnsureSuccessStatusCode();
            var responseBody = response.Content.ReadAsString();

            Debug.WriteLine(responseBody);
        }
    }
}
