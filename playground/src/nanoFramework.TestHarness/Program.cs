using nanoFramework.DependencyInjection;
using nanoFramework.Networking;
using nanoFramework.Hosting.Logging;
using nanoFramework.Hosting.Logging.Debug;
using nanoFramework.Hosting;
using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using nanoFramework.Hosting.Identity;
using System.Text;

namespace nanoFramework.TestHarness
{
    // nanoff --target M5Core2 --serialport COM9 --update

    public class Program
    {
        public static void Main()
        {
            string username = "b.smith";
            byte[] password = Encoding.UTF8.GetBytes("password");

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddIdentity(username, password);
                })
                .ConfigureIdentity(provider => 
                {
                    var user = provider.FindByName(username);
                    provider.UpdatePassword(user, Encoding.UTF8.GetBytes("password2"), true);
                }).Build();
            
            var identityProvider = (IdentityProvider)host.Services.GetRequiredService(typeof(IIdentityProvider));

            var user = identityProvider.FindByName(username);

            Debug.WriteLine(user.UserName);
            Debug.WriteLine(identityProvider.VerifyPassword(user, Encoding.UTF8.GetBytes("password2")).Succeeded.ToString());

            //var serviceProvider = new ServiceCollection()
            //.AddLogging(builder =>
            //{
            //    builder.AddDebug();
            //    builder.SetMinimumLevel(LogLevel.Information);
            //})
            //.BuildServiceProvider();


            //var builder = HostApplication.CreateBuilder();
            //builder.Logging.AddDebug();

            //var app = builder.Build();
            //app.Run();

            ////SetupWifi();

            //var serviceProvider = new ServiceCollection()
            //.AddLogging(builder =>
            //{
            //    builder.AddDebug();
            //    builder.SetMinimumLevel(LogLevel.Information);
            //})
            //.BuildServiceProvider();

            //var loggerFactory = (ILoggerFactory)serviceProvider.GetRequiredService(typeof(ILoggerFactory));
            //var logger = loggerFactory.CreateLogger("name");
            //logger.Log(LogLevel.Information, "Hello!!!");


            //var builder = HostApplication.CreateBuilder();
            //builder.Logging.AddDebug();

            //var app = builder.Build();
            //app.Logger.Log(LogLevel.Information, "Hello!!!");
            //app.Run();

        }

        //private static void SetupWifi()
        //{
        //    var success = WifiNetworkHelper.ConnectDhcp("ssid", "password");

        //    if (success)
        //    {
        //        Debug.WriteLine("IP: " + NetworkInterface.GetAllNetworkInterfaces()[0].IPv4Address);
        //    }
        //    else
        //    {
        //        Debug.WriteLine($"Failed to establish IP address and DateTime, error: {NetworkHelper.HelperException}.");
        //    }
        //}
    }
}